#define RAM static ram
#define TRUE 1
#define FALSE 0
#define WORD unsigned int
#define BIT unsigned char

// Define Software UART Speed  (Trace)
#define BPS38400

// Define processor clock speed
#define CLOCK8MHZ


#include "stdlib.h"
#include "stdio.h"
#include "math.h"
#include "delays.h"
#include <pconfig.h>
#include "timers.h"
#include "p18cxxx.h"
#include "Myusart.h"


extern char volatile MsgReady;
extern int volatile NQueue;
extern unsigned char volatile Buffer[MaxPacketLength * 2];
extern unsigned int volatile QHead;
extern unsigned int volatile QTail; 
extern unsigned int volatile pMsg ;
extern unsigned char uartdata;
ram unsigned char volatile CUart;
ram unsigned int volatile PadCount;    // Number of Message Start Pad characters.   >3 = Hard Reset
ram unsigned long volatile TimerPopCount;
ram int volatile Hdr;
ram int volatile DataLen[3];
extern int volatile UARTErrorOR;
extern int volatile UARTErrorFR;
ram int volatile iStarting;
ram unsigned char * Qptr;

void WriteUART(char *);

void MyISR (void);


#pragma code HighVector=0x08
void HighVector (void)
{
   _asm
     goto MyISR
   _endasm
 }

#pragma code LowVector=0x18
void LowVector (void)
{
   _asm
     goto MyISR
   _endasm
 }

#pragma code

// Use Low Priority interrupt to prevent compiler from using retfile FAST.
// This is a workaround for a bug in the PIC hardware.  See errata sheet.

#pragma interruptlow MyISR

void MyISR (void)
{
 

 if(PIR1bits.RCIF)  // If RCIF is set
{
 

   // Endble Watchdog timer.  Data must be serviced within approx 512MS or processor is reset by WDT.
   WDTCON = 1;

   USART_Status.val &= 0xf0;         // Clear previous status flags
   
   if(RCSTAbits.FERR)                 // If a framing error occured
     {
        USART_Status.FRAME_ERROR = 1;    // Set the status bit
        UARTErrorFR += 1;
        RCSTAbits.CREN = 0;
        CUart = 0;
        iStarting = TRUE;
        goto exit;
     }

   if(RCSTAbits.OERR)                 // If an overrun error occured
     {
        USART_Status.OVERRUN_ERROR = 1;  // Set the status bit
        UARTErrorOR += 1;
        RCSTAbits.CREN = 0;
        CUart = 0;
        iStarting = TRUE;
        goto exit;
     }

 
     CUart= (unsigned char) RCREG;


     if (CUart == 0xff) 
     {
        Hdr = 1; 
        PadCount ++;
        iStarting = FALSE;
        goto exit;
     }

       
     if(Hdr == 1)
     {
       
        PadCount = 0;
        Hdr = 0;
        Qptr = Buffer + (QHead * MaxPacketLength);
        *Qptr = CUart;
        DataLen[QHead] = (int)*Qptr;
        Qptr++;
        goto exit;
     }  
 
     if(iStarting == TRUE) goto exit;

     *Qptr = CUart;
     Qptr++;  

     if(Qptr > (Buffer +(QHead * MaxPacketLength) + DataLen[QHead]))
     {
       
        QHead ++;
        if (QHead > 1) 
        {
           QHead = 0;
        } 
        NQueue ++;
      }  

       
}
exit:

// Check for Timer0 overflow
    if (INTCONbits.TMR0IF)
    {

       // x'06A5' written to timer to set interrupt period to 32MS
       TMR0H = 0x06;
       TMR0L = 0xA5;
       INTCONbits.TMR0IF = 0;
       TimerPopCount += 1;
    }

    
		
  
   
}



#pragma code

int vgetsUSART(char buffer[MaxPacketLength], int len, int TOT)
 {
       
    int Len;

     // Interrupt driven version
     if(NQueue > 0)
     {

     
        memcpy(buffer,( void *) &Buffer+((QTail) * MaxPacketLength)+ 1,DataLen[QTail]);
        memset(buffer+DataLen[QTail],'\0',1);
        memset((void *)Buffer+(QTail*MaxPacketLength),'\0',MaxPacketLength);
        Len = DataLen[QTail];
        DataLen[QTail] = 0;
        QTail ++;
        if (QTail > 1) QTail = 0;
        NQueue --;
        return Len;
      }
     else
     {
        return -1;
     }
 }




unsigned long GetTimeMS(void)
{
   ram unsigned long MS, Elapsed;
   union Timers timer0;
       timer0.bt[0] = TMR0L;  // Copy Timer0 low byte into union
       timer0.bt[1] = TMR0H; 
       Elapsed = timer0.lt;
       MS = (TimerPopCount * 32) + ((Elapsed ) / 2000);
       return MS;
}

unsigned long ResetTimer1(void)
{

    unsigned long Elapsed, Remainder,  MS;

    // Elapsed time in MS since last reset.
    union Timers timer;
    timer.bt[0] = TMR1L;  // Copy Timer1 low byte into union
    timer.bt[1] = TMR1H; 
    Elapsed = timer.lt;
    MS = (Elapsed / 250) ;
    Remainder = Elapsed - (MS * 250) + 54 ;
    WriteTimer1(Remainder);
    return(MS);
}


void WriteTimer0(unsigned int timer0)

{

union Timers timer;

   timer.lt = timer0; // Copy timer value into union
   TMR0H = timer.bt[1]; // Write low byte to Timer0
   TMR0L = timer.bt[0]; // Write high byte to Timer0

}



#define getcUART ReadUART


#define putcUART WriteUART
 //#define putcUART(a)        WriteUART(a) 
//#define putcUART(a) WriteUART(a)

//void putcUART( char *cdata)
//{
//	uartdata = (char) *cdata;
//	WriteUART;
//}

int getsUSART_TOT(char *buffer, unsigned int len, unsigned int MS);

//////////////////////////////////////////////////////////////////////////////


/* Change this to near if building small memory model versions of
 * the libraries. */
#define MEM_MODEL far

/* storage class of library routine parameters; pre-built with auto;
 * do not change unless you rebuild the libraries with the new storage class */ 
#define PARAM_SCLASS auto




//===================================================
// Fixes for Software
//===================================================


// Fix for strncpy
char* strncpy (char *s1, const char *s2, size_t n)
{
    char *s;
    for (s=s1; 0 < n && *s2!= '\0'; --n)
        *s++ = *s2++;
    for (;0<n; --n)
        *s++ = '\0';
    return(s1);
}



// Fix for strncmp

signed char strncmp (const char *s1, const char *s2, size_t n)
{

    for (; 0<n; ++s1, ++s2, --n)
        if (*s1 != *s2)
            return((*(unsigned char *) s1
                    < *(unsigned char *)s2) ? -1: +1);
        else if (*s1 == '\0')
            return(0);
    return(0);
}



// fix for memcpy

void *(memcpy) (void *s1, const void *s2, size_t n)
{
   char *su1;
   const char *su2;

   for(su1 = s1, su2 = s2; 0<n; ++su1, ++su2, --n)
   {
      *su1 = *su2;
      
   }
return(s1);
}





void OpenUSARTpic18f2525( unsigned int speed)
{
    TXSTA = 0;           // Reset USART registers to POR state

    RCSTA = 0;

    RCSTAbits.CREN = 1;    // Continuous Receive

    TXSTAbits.BRGH = 1;    // Baud Rate High

    PIR1bits.TXIF = 0;     

    PIE1bits.RCIE = 1;     // Receive interrupt

    PIE1bits.TXIE = 0;     //No Transmit Interrupts

    // For 8 MHZ Clock

    if (speed == 38400)
    {
        SPBRG = 52;
    }

    if (speed == 57600)
    {
        SPBRG = 34;
    }

    
    BAUDCON = 0x08;        // Set BRG16

    TXSTAbits.BRGH = 1;    // High Baud Rate 
    TRISCbits.TRISC6 = 1;
    TRISCbits.TRISC7 = 1; 

    RCONbits.IPEN   = 1;   // Enable Interrupt Priority
    IPR1bits.RCIP   = 0;   // Receive Interrupts are High Priority
    INTCONbits.PEIE = 1;   // Enable Priorities
    INTCONbits.GIEH = 1;   // Enable High Priority Interrupts
        

    TXSTAbits.TXEN = 1;    // Enable transmitter

    RCSTAbits.SPEN = 1;    // Enable receiver


}


/**********************************************************************
*    Function Name:  putsUART                                         *
*    Return Value:   void                                             *
*    Parameters:     data: pointer to string of data                  *
*    Description:    This routine transmits a string of characters    *
*                    to the UART data byte including the null.        *
**********************************************************************/

void putsUART( char *cdata)
{
    // Modified P. Cranwell to write all except final null.
    int i, Len;
    char c;

    Len = strlen(cdata);

    for (i=0; i<Len; i++)
    {
        c = *cdata;
        //uartdata = *cdata;
	       
        putcUART(c);
        *cdata++;
    }

}

/**********************************************************************
*    Function Name:  vputsUART                                        *
*    Return Value:   void                                             *
*    Parameters:     data: pointer to string of data                  *
*    Description:    This routine transmits a string of characters    *
*                    prefixed by a byte length code that defines the  *
*                    length of the remaining message                  *
**********************************************************************/
void vputsUART( char *cdata)
{
    // Modified P. Cranwell to write all except final null.
    int i, Len;
    int Reset = 255;
    char WorkMsg[73];

    Len = strlen(cdata);
    memset(WorkMsg,'\0',sizeof(WorkMsg));
    memcpy((void *)&WorkMsg[1],(void *)&Reset,1);
    memcpy((void *)&WorkMsg[2],(void *)&Len,1);
    strcat(WorkMsg,cdata);
    putsUSART(WorkMsg);
}


//************************  USART Fuctions *****************************

/* **********************************************
 * Function Name: baudUSART                     *
 * Parameters: baudconfig                       *
 *             Configuration setting for the    *
 *             BAUDCTL register.                *
 * Description: sets the configuration for the  *
 *              BAUDCTL register.               *
 * **********************************************/

#if defined (EAUSART_V3) || defined (EAUSART_V4) || defined (EAUSART_V5)
void baudUSART (unsigned char baudconfig)
{
    BAUDCON = baudconfig;
}

/*********************************************************************
*    Function Name:  BusyUSART                                       *
*    Return Value:   char: transmit successful status                *
*    Parameters:     none                                            *
*    Description:    This routine checks to see if a byte can be     *
*                    written to the USART by checking the TRMT bit   *
*********************************************************************/
    #if defined (AUSART_V1) || defined (EAUSART_V3) || defined (EAUSART_V4) || defined (EAUSART_V5)
        #undef BusyUSART
char BusyUSART(void)
{
    if (!TXSTAbits.TRMT)  // Is the transmit shift register empty
        return 1;          // No, return FALSE
    return 0;            // Return TRUE
}

    #endif





/**********************************************************************
*    Function Name:  putsUSART                                        *
*    Return Value:   void                                             *
*    Parameters:     data: pointer to string of data                  *
*    Description:    This routine transmits a string of characters    *
*                    to the USART including the null.                 *
**********************************************************************/

    #if defined (AUSART_V1) || defined (EAUSART_V3) || defined (EAUSART_V4) || defined (EAUSART_V5)

void putsUSART( char *data)
{

    do
    {  // Transmit a byte
        while (BusyUSART());
        putcUSART(*data);
    } while ( *data++ );
}

void vputsUSART( char *data, int len)
{
   
// Write variable message prefixed with single byte length code
// (C) 2010 P. Cranwell
   char *p;
   char *q;
   char Reset = 255;
   int i;

        
          i = len;

          // Create pointers & length code
          q = (char *) &len;
         
          memset(WorkBuffer,'\0',sizeof(WorkBuffer));
          memcpy(WorkBuffer,(void *) &Reset,1);
          memcpy((void *)&WorkBuffer[1],(void *)&i,1);                        // add length code
          memcpy((void *)&WorkBuffer[2] ,(void *) data,len);             // Copy Data

         
          p = WorkBuffer;

         
         
          for(i=0; i<len+2; i++)                         // Write Variable Message
          {  // Transmit a byte
             while(BusyUSART());
             putcUSART(*p);
             p++;
          }
      
}

#endif


/********************************************************************
*    Function Name:  DataRdyUSART                                   *
*    Return Value:   char                                           *
*    Parameters:     void                                           *
*    Description:    This routine checks the RCIF flag to see if    *
*                    any data has been received by the USART. It    *
*                    returns a 1 if data is available and a 0 if    *
*                    not.                                           *
********************************************************************/
#if defined (AUSART_V1) || defined (EAUSART_V3) || defined (EAUSART_V4) || defined (EAUSART_V5)
#undef DataRdyUSART
          char DataRdyUSART(void)
{
          if (PIR1bits.RCIF)  // If RCIF is set
          return 1;  // Data is available, return TRUE
          return 0;  // Data not available, return FALSE
}

#endif







/**********************************************************************
*    Function Name:  putrsUSART                                       *
*    Return Value:   void                                             *
*    Parameters:     data: pointer to string of data                  *
*    Description:    This routine transmits a string of characters    *
*                    in ROM to the USART including the null.          *
**********************************************************************/
#if defined (AUSART_V1) || defined (EAUSART_V3) || defined (EAUSART_V4) || defined (EAUSART_V5)

          void putrsUSART(const far rom char *data){
          do{  // Transmit a byte
          while (BusyUSART());
          putcUSART(*data);} while ( *data++ );
}

#endif

/********************************************************************
*    Function Name:  ReadUSART                                      *
*    Return Value:   char: received data                            *
*    Parameters:     void                                           *
*    Description:    This routine reads the data from the USART     *
*                    and records the status flags for that byte     *
*                    in USART_Status (Framing and Overrun).         *
********************************************************************/
#if defined (AUSART_V1) || defined (EAUSART_V3) || defined (EAUSART_V4) || defined (EAUSART_V5)

          unsigned char MyReadUSART(void)        //this function can be removed by macro #define ReadUSART RCREG
{
          unsigned char data;   // Holds received data

          USART_Status.val &= 0xf2;          // Clear previous status flags

          if (RCSTAbits.RX9)                  // If 9-bit mode
          {
             USART_Status.RX_NINE = 0;        // Clear the receive bit 9 for USART

             if (RCSTAbits.RX9D)               // according to the RX9D bit
             USART_Status.RX_NINE = 1;
          }

          if (RCSTAbits.FERR)                 // If a framing error occured

          {
             USART_Status.FRAME_ERROR = 1;    // Set the status bit
             RCSTAbits.CREN=0;
             RCSTAbits.CREN=1;
          } 

          if (RCSTAbits.OERR)                 // If an overrun error occured
          {
             USART_Status.OVERRUN_ERROR = 1;  // Set the status bit
             RCSTAbits.CREN=0;
             RCSTAbits.CREN=1;
          } 

          data = (unsigned char) RCREG;                      // Read data

          return(data);                     // Return the received data
}

#endif


/********************************************************************
*    Function Name:  WriteUSART                                     *
*    Return Value:   none                                           *
*    Parameters:     data: data to transmit                         *
*    Description:    This routine transmits a byte out the USART.   *
********************************************************************/

#if defined (AUSART_V1) || defined (EAUSART_V3) || defined (EAUSART_V4) || defined (EAUSART_V5)

      void WriteUSART(char data)
      {
          if (TXSTAbits.TX9)  // 9-bit mode?
          {
             TXSTAbits.TX9D = 0;       // Set the TX9D bit according to the
             if (USART_Status.TX_NINE)  // USART Tx 9th bit in status reg
                TXSTAbits.TX9D = 1;
          }

          TXREG = data;      // Write the data byte to the USART
      }



#endif



//***********************************************************************





// ============================== 38400 BPS ==================================

#ifdef BPS38400 
#ifdef CLOCK8MHZ
          void DelayTXBitUART(){
          // for 38400 BPS and 8MHZ Clock 
          // Delay 37 Instruction Cycles. (MCC18 Source code calls for 41 but it is incorrect)
          Delay10TCYx(3);
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();
        

         // Note: Microchip Sample Software is incorrect re TX Bit Delay.

}


          void DelayRXHalfBitUART(){
          // for 38400 @ 8 MHZ
          // Delay 18 Instruction Cycles
          Delay10TCYx(1);
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();

}

          void DelayRXBitUART(){
          // for 38400 BPS @ 8 MHZ
          // Delay 40 Instruction Cycles
          Delay10TCYx(4);}
#endif
#endif

#ifdef BPS38400 
#ifdef CLOCK40MHZ
          void DelayTXBitUART(){  
          // 38400 BPS @ 40MHZ Clock 
          // Delay 248 Instruction Cycles
          Delay10TCYx(24);
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();


}
          void DelayRXHalfBitUART(){

          // for 38400 @ 40 MHZ
          // Delay 122 Instruction Cycles
          Delay10TCYx(12);
          Delay1TCY();
          Delay1TCY();


}

          void DelayRXBitUART(){
          // for 38400 BPS @ 40 MHZ
          // Delay 247 Instruction Cycles
          Delay10TCYx(24);
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();}
#endif
#endif



#ifdef BPS9600 
#ifdef CLOCK8MHZ
          void DelayTXBitUART(){
          // for 9600 BPS and 8MHZ Clock 
          Delay10TCYx(19);
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();}
          void DelayRXHalfBitUART(){
          // for 9600 @ 8 MHZ
          Delay10TCYx(9);
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();}

          void DelayRXBitUART(){
          // for 38400 BPS @ 8 MHZ
          Delay10TCYx(19);
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();}
#endif
#endif


#ifdef BPS9600 
#ifdef CLOCK40MHZ
          void DelayTXBitUART(){
          // for 9600 BPS and 40MHZ Clock 
          // Delay 1030 Instruction 
          Delay10TCYx(103);

}
          void DelayRXHalfBitUART(){
          // for 9600 @ 40 MHZ
          // Delay 512 Instruction Cycles
          Delay10TCYx(51);
          Delay1TCY();
          Delay1TCY();


}

          void DelayRXBitUART(){
          // for 9600 BPS @ 40 MHZ
          // Delay 1028 Instruction Cycles
          Delay10TCYx(102);
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();
          Delay1TCY();}
#endif
#endif









          void SetClockSpeed(unsigned int speed) {

          if (speed == 8){
          OSCTUNE = 0x00;
          OSCCON =  0x72;  // Set internal 8Mhz clock
          }

          if (speed == 1){
          OSCTUNE = 0x00;
          OSCCON =  0x42;  // Set internal 1Mhz clock
          }

          if (speed == 40){
          OSCTUNE = 0x00;
          OSCCON =  0x00;  // Set External clock}
          }
        }


          void DelayMS(unsigned long MS){

         // Callibrated for 8MHZ Clock
          unsigned long  i;

          for (i = 0 ; i<MS; i++){
            Delay10TCYx(196);
            ClrWdt()
            }
          }


// Configuration for PIC18F2525

          void Pin3HIGH(void){
          // Port A, Bit 1
          TRISAbits.TRISA1 = 0;
          LATAbits.LATA1=1;
          }

          void Pin3LOW(void){
          //PORT A, Bit 1
          TRISAbits.TRISA1 = 0;
          LATAbits.LATA1=0;
         
      	  }

          BIT GetPin3(void){
          // Change PORT A , Bit 1 to Input
          TRISAbits.TRISA1 = 1;
          return(PORTAbits.RA1);
          }




          void Pin4HIGH(void){
          // Port A, Bit 2
          TRISAbits.TRISA2=0;	
          LATAbits.LATA2=1;
          }

          void Pin4LOW(void){
          //PORT A, Bit 2
          TRISAbits.TRISA2=0;
          LATAbits.LATA2=0;
          }
  
          BIT GetPin4(void){
          // Change PORT A , Bit 2 to Input
          TRISAbits.TRISA2=1;	
          return(PORTAbits.RA2);
          }




          void Pin5HIGH(void){
          // Port A, Bit 3
          TRISAbits.TRISA3 = 0;
		  LATAbits.LATA3=1;
		  }
							

          void Pin5LOW(void){
          //PORT A, Bit 3
          TRISAbits.TRISA3=0;
		  LATAbits.LATA3=0;
          }

          BIT GetPin5(void){
          // Change PORT A , Bit 3 to Input
          TRISAbits.TRISA3=1;
          return(PORTAbits.RA3);
          }




          void Pin6HIGH(void){
          // Port A, Bit 4
          TRISAbits.TRISA4=0;
          LATAbits.LATA4=1;
          }

          void Pin6LOW(void){
          //PORT A, Bit 4
          TRISAbits.TRISA4=0;
	      LATAbits.LATA4=0;
          }

          BIT GetPin6(void){
          // Change PORT A , Bit 4 to Input
          TRISAbits.TRISA4=1;
		  return(PORTAbits.RA4);
		  }





          void Pin7HIGH(void){
          // Port A, Bit 5
          TRISAbits.TRISA5 = 0;
		  LATAbits.LATA5 = 1;
		  }

          void Pin7LOW(void){
          //PORT A, Bit 5
          TRISAbits.TRISA5=0;
		  LATAbits.LATA5=0;
		  }

          BIT GetPin7(void){
          // Change PORT A , Bit 5 to Input
          TRISAbits.TRISA5=1;
		  return(PORTAbits.RA5);		
		  }




          void Pin9HIGH(void){
          // Port A, Bit 7
          TRISAbits.TRISA7 = 0;
          LATAbits.LATA7=1;
		  }

          void Pin9LOW(void){
          //PORT A, Bit 7
          TRISAbits.TRISA7=0;
		  LATAbits.LATA7=0;
		  }

          BIT GetPin9(void){
          // Change PORT A , Bit 7 to Input
          TRISAbits.TRISA7=1;
		  return(PORTAbits.RA7);
		  }





          void Pin10HIGH(void){
          // Port A, Bit 6
          TRISAbits.TRISA6 = 0;
		  LATAbits.LATA6=1;
		  }

          void Pin10LOW(void){
          //PORT A, Bit 6
          TRISAbits.TRISA6=0;
		  LATAbits.LATA6=0;
		  }

          BIT GetPin10(void){
          // Change PORT A , Bit 6 to Input
          TRISAbits.TRISA6=1;
		  return(PORTAbits.RA6);
		  }





          void Pin11HIGH(void){
          // Port C, Bit0 
          TRISCbits.TRISC0 = 0;
		  LATCbits.LATC0=1;
	  	  }

          void Pin11LOW(void){
          //PORT C, Bit 0
          TRISCbits.TRISC0=0;
		  LATCbits.LATC0=0;
		  }

          BIT GetPin11(void){
          // Change PORT C , Bit 0 to Input
          TRISCbits.TRISC0=1;
		  return(PORTCbits.RC0);
		  }





          void Pin12HIGH(void){
          // PORT C, Bit 1
          TRISCbits.TRISC1=0;
		  LATCbits.LATC1=1;
		  }

          void Pin12LOW(void){
          //PORT C, Bit 1
          TRISCbits.TRISC1=0;
		  LATCbits.LATC1=0;
		  }

          BIT GetPin12(void){
          // Change PORT C , Bit 1 to Input
          TRISCbits.TRISC1=1;
		  return(PORTCbits.RC1);
		  }





          void Pin13HIGH(void){
          // PORT C, Bit 2
          TRISCbits.TRISC2=0;
		  LATCbits.LATC2=1;
		  }

          void Pin13LOW(void){
          //PORT C, Bit 2
          TRISCbits.TRISC2=0;
		  LATCbits.LATC2=0;
		  }

          BIT GetPin13(void){
          // Change PORT C , Bit 2 to Input
          TRISCbits.TRISC2=1;
		  return(PORTCbits.RC2);
		  }





          void Pin14HIGH(void){
          // PORT C, Bit 3
          TRISCbits.TRISC3=0;
		  LATCbits.LATC3=1;
		  }

          void Pin14LOW(void){
          //PORT C, Bit 3
          TRISCbits.TRISC3=0;
		  LATCbits.LATC3=0;
		  }

          BIT GetPin14(void){
          // Change PORT C , Bit 3 to Input
          TRISCbits.TRISC3=1;
		  return(PORTCbits.RC3);
		  }





          void Pin15HIGH(void){
          // PORT C, Bit 4
          TRISCbits.TRISC4=0;
		  LATCbits.LATC4=1;
		  }

          void Pin15LOW(void){
          //PORT C, Bit 4
       	  TRISCbits.TRISC4=0;
		  LATCbits.LATC4=0;
		  }

          BIT GetPin15(void){
          // Change PORT C , Bit 4 to Input
          TRISCbits.TRISC4=1;
		  return(PORTCbits.RC4);
		  }





          void Pin16HIGH(void){
          // PORT C, Bit 5
          TRISCbits.TRISC5=0;
		  LATCbits.LATC5=1;
		  }

          void Pin16LOW(void){
          //PORT C, Bit 5
          TRISCbits.TRISC5=0;
		  LATCbits.LATC5=0;
		  }

          BIT GetPin16(void){
          // Change PORT C , Bit 5 to Input
          TRISCbits.TRISC5=1;
		  return(PORTCbits.RC5);
		  }






          void Pin21HIGH(void){
          // PORT B, Bit0 
          TRISBbits.TRISB0=0;
		  LATBbits.LATB0=1;
		  }

          void Pin21LOW(void){
          //PORT B, Bit 0
          TRISBbits.TRISB0=0;
		  LATBbits.LATB0=0;
		  }

          BIT GetPin21(void){
          // Bhange PORT B , Bit 0 to Input
          TRISBbits.TRISB0=1;
		  return(PORTBbits.RB0);
		  }







          void Pin22HIGH(void){
          // PORT B, Bit 1
		  TRISBbits.TRISB1=0;
		  LATBbits.LATB1=1;
		  }

          void Pin22LOW(void){
          //PORT B, Bit 1
          TRISBbits.TRISB1=0;
		  LATBbits.LATB1=0;
		  }

          BIT GetPin22(void){
          // Change PORT B , Bit 1 to Input
          TRISBbits.TRISB1=1;
		  return(PORTBbits.RB1);
		  }






          void Pin23HIGH(void){
          // PORT B, Bit 2
          TRISBbits.TRISB2=0;
		  LATBbits.LATB2=1;
		  }

          void Pin23LOW(void){
          //PORT B, Bit 2
          TRISBbits.TRISB2=0;
		  LATBbits.LATB2=0;
		  }

          BIT GetPin23(void){
          // Change PORT B , Bit 2 to Input
          TRISBbits.TRISB2=1;
		  return(PORTBbits.RB2);
		  }







          void Pin24HIGH(void){
          // PORT B, Bit 3
          TRISBbits.TRISB3=0;
		  LATBbits.LATB3=1;
		  }

          void Pin24LOW(void){
          //PORT B, Bit 3
          TRISBbits.TRISB3=0;
		  LATBbits.LATB3=0;
		  }

          BIT GetPin24(void){
          // Change PORT B , Bit 3 to Input
          TRISBbits.TRISB3=1;
		  return(PORTBbits.RB3);
		  }






          void Pin25HIGH(void){
          // PORT B, Bit 4
          TRISBbits.TRISB4=0;
		  LATBbits.LATB4=1;
		  }

          void Pin25LOW(void){
          //PORT B, Bit 4
          TRISBbits.TRISB4=0;
		  LATBbits.LATB4=0;
		  }

          BIT GetPin25(void){
          // Change PORT B , Bit 4 to Input
          TRISBbits.TRISB4=1;
		  return(PORTBbits.RB4);
		  }





          void Pin26HIGH(void){
          // PORT B, Bit 5
          TRISBbits.TRISB5=0;
		  LATBbits.LATB5=1;
		  }

          void Pin26LOW(void){
          //PORT B, Bit 5
          TRISBbits.TRISB5=0;
		  LATBbits.LATB5=0;
		  }

          BIT GetPin26(void){
          // Change PORT B , Bit 5 to Input
          TRISBbits.TRISB5=1;
		  return(PORTBbits.RB5);
		  }






          void Pin27HIGH(void){
          // PORT B, Bit 6
          TRISBbits.TRISB6=0;
		  LATBbits.LATB6=1;
		  }

          void Pin27LOW(void){
          //PORT B, Bit 6
          TRISBbits.TRISB6=0;
		  LATBbits.LATB6=0;
		  }

          BIT GetPin27(void){
          // Change PORT B , Bit 6 to Input
          TRISBbits.TRISB6=1;
		  return(PORTBbits.RB6);
		  }






          void Pin28HIGH(void){
          // PORT B, Bit 7
          TRISBbits.TRISB7=0;
		  LATBbits.LATB7=1;
		  }

          void Pin28LOW(void){
          //PORT B, Bit 7
          TRISBbits.TRISB7=0;
		  LATBbits.LATB7=0;
		  }

          BIT GetPin28(void){
          // Change PORT B , Bit 7 to Input
          TRISBbits.TRISB7=1;
		  return(PORTBbits.RB7);
		  }














          void PinHIGH(int Pin){
          switch (Pin){

          case 3:
             Pin3HIGH();
             break;
          case 4:
             Pin4HIGH();
             break;
          case 5:
             Pin5HIGH();
             break;
          case 6:
             Pin6HIGH();
             break;
          case 7:
             Pin7HIGH();
             break;
          case 9:
             Pin9HIGH();
             break;
          case 10:
             Pin10HIGH();
             break;
          case 11:
             Pin11HIGH();
             break;
          case 12:
             Pin12HIGH();
             break;
          case 13:
             Pin13HIGH();
             break;
          case 14:
             Pin14HIGH();
             break;
          case 15:
             Pin15HIGH();
             break;
          case 16:
             Pin16HIGH();
             break;
          case 21:
             Pin21HIGH();
             break;
          case 22:
             Pin22HIGH();
             break;
          case 23:
             Pin23HIGH();
             break;
          case 24:
             Pin24HIGH();
             break;
          case 25:
             Pin25HIGH();
             break;
          case 26:
             Pin26HIGH();
             break;
          case 27:
             Pin27HIGH();
             break;
          case 28:
             Pin28HIGH();
          break;
        }



}



          void PinLOW(int Pin){
          switch (Pin)
			{
			case 3:
         		 Pin3LOW();
         		 break;	
			case 4:
          		Pin4LOW();
          		break;
			case 5:
          		Pin5LOW();
          		break;
			case 6:
          		Pin6LOW();
          		break;
			case 7:
          		Pin7LOW();
          		break;
			case 9:
          		Pin9LOW();
          		break;
			case 10:
         		 Pin10LOW();
          		break;
			case 11:
          		Pin11LOW();
          		break;
			case 12:
          		Pin12LOW();
          		break;
			case 13:
          		Pin13LOW();
          		break;
 			case 14:
          		Pin14LOW();
          		break;
 			case 15:
          		Pin15LOW();
          		break;
			case 16:
          		Pin16LOW();
          		break;
			case 21:
          		Pin21LOW();
          		break;
			case 22:
          		Pin22LOW();
          		break;
			case 23:
          		Pin23LOW();
          		break;
			case 24:
          		Pin24LOW();
          		break;
 			case 25:
          		Pin25LOW();
          		break;
			case 26:
          		Pin26LOW();
          		break;
			case 27:
          		Pin27LOW();
          		break;
			case 28:
          		Pin28LOW();
          		break;
			}

		}





          int PinTest(int Port){
          switch (Port){
			case 3:
          		return GetPin3();
          		break;
			case 4:
          		return GetPin4();
          		break;
			case 5:
          		return GetPin5();
          		break;
			case 6:
          		return GetPin6();
         		break; 
			case 7:
          		return GetPin7();
          		break;
			case 9:
          		return GetPin9();
          		break;
			case 10:
          		return GetPin10();
          		break;
			case 11:
          		return GetPin11();
         	 	break;
			case 12:
          		return GetPin12();
          		break;
			case 13:
          		return GetPin13();
         		break; 
			case 14:
          		return GetPin14();
         		break;
			case 15:
          		return GetPin15();
         		break;
			case 16:
          		return GetPin16();
          		break;
			case 21:
          		return GetPin21();
          		break;
			case 22:
          		return GetPin22();
          		break;
			case 23:
          		return GetPin23();
          		break;
			case 24:
          		return GetPin24();
         		break;
			case 25:
          		return GetPin25();
          		break;
			case 26:
          		return GetPin26();
          		break;
			case 27:
          		return GetPin27();
          		break;
			case 28:
          		return GetPin28();
          		break;

					}
			}





          void Pause (int MS){
          // Assumes a 40 Mhz clck
          Delay10KTCYx(MS);
		  }



          int abs (int i){
          return((i<0) ? -i : i);
          }




