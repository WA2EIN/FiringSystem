/*****************************************************************************
'
' Hand Controller     
' Author  P. Cranwell             Date  10/7/2011
'
' Processor:  Microchip PIC18F25K80   Dual USART.
'                        
'~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
'
'  Hardware Pin Usage
'
'  Pin 1  - Master Reset
'  Pin 2  - Keyboard
'  Pin 3  - Keyboard
'  Pin 4  - Keyboard
'  Pin 5  - Keyboard
'  Pin 6  - +5
'  Pin 7
'  Pin 8  - Gnd
'  Pin 9  - OSC1   10/40 MHZ
'  Pin 10 - OSC1 
'  Pin 11 - OSC2   31KHZ  TOD Clock
'  Pin 12 - OSC2
'  Pin 13 - OK LED
'  Pin 14 - NG LED
'  Pin 15 - RS485 - Transmit Enable
'  Pin 16 - Armed LED 
'  Pin 17 - TX1 - Xbee Transmit Data   38400 BPS
'  Pin 18 - RX1 - Xbee Receive Data
'  Pin 19 - Gnd
'  Pin 20 - +5
'  Pin 21 - Keyboard
'  Pin 22 - Keyboard
'  Pin 23 - Keyboard
'  Pin 24 - Keyboard
'  Pin 25 - Signal LED
'  Pin 26 - Pickle Switch / Foot Pedal
'  Pin 27 - Trace/LCD  9600 BPS
'  Pin 28 - Active LED
' 
'   
'
'
'  Hand Controller Radio Linked Pyrotechnic Firing System
'  Using XBee Radio.
' 
'  The program is a Hand Fired Controller.  
'   It generates commands and sends them to a slave remote.
'   It accepts inpout from a 16 key keypad, a toggle switch and displays 
'     information on a SeeTron GLO-216 OLED serial display.
'
'  Message Structure
'
'   All messages are variable length in the following format:
'   Message 1 format:
'   unsigned char  1      Length Code Binary (Defining length of following message (1-255 bytes)
'
'  
'   Message format:
'
'   unsigned char  1      MN = Message Number Modulo 10
'   unsigned char  2      Number of  14 byte Work Units
'
'   unsigned char  3-4 Unit
'                      00 = Reserved
'                      01 = Host
'                      02 = Reserved
'                      03..nn Slave Slats (16 Cues each)
'                      99   All Units Broadcast
'                      01 = Start
'                      02 = Stop
'                      04 = Fire
'                      05 = Sense
'                      06 = Trace ON
'                      07 = ACK
'                      08 = NAK
'                      09 = Triggered Show Events message (Containing up to 16TWU)
'                      10 = Arm
'                      11 = Disarm
'                      12 = Measure HV
'                      13 = Set Default Show
'                      14 = Save Show in EEPROM
'                      08 = Load Show from EEPROM
'                      19 = Trace OFF
'                      17 = Start of Programming Triggered Show
'                      18 = End of Programming
'                      33 = Set Unit Address into EEPROM (Broadcast)
'                      44 = Trigger Cue                  (Broadcast)  
'                      55 = Start Show Clock             (Broadcast)
'                      66 = Clock Callibrate Start       (Broadcast)
'                      77 = Clock Callibrate Stop        (Broadcast)
'                      98 = Current Time                 (Broadcast)
'                      99 = Reset                        (Broadcast)  
'
'   unsigned char 7-8  Port Number
'                      00 = Reserved
'                      01-nn are pin numbers
'
'   unsigned char 9-16 Time Code MMMMMMMM  Milliseconds.
'
'                 
'   Last Byte          Check Digit
'
'
'
'====================================================================================
' ACK Format
'
'   unsigned char  1     MN = Same Message Number as received message
'   unsigned char  2     WU = Number of Work Units = 1
'   unsigned charS 3-4   ACK Command Code
'   unsigned char  5     Message Check Code
'====================================================================================
' NAK Format
'
'   unsigned char  1      MN = Same Message Number as received message
'   unsigned char  2      WU = Number of Work Units 
'   unsigned charS 3-4    NAK Ccooand Code
'   unsigned char  5      Message Check Code
'====================================================================================
' PC Master, Relay controller and Slaves  maintain message counters.
'
'  Each Transmitter maints a next send message number.
'
'  8/13/08 V1L1  Added Piano Support, sequentiial Que firing.
'  7/23/09 V1L2  Added Serial Interface & Radio Repeat function.
'  7/10/10 V1L4  Corrected Piano/Pickle switch logic.
'  7/18/10 V1L4  Increased size of BIGDATA section, modified linker file.
'  4/27/11 V1L5  Added VSend / VReceive for XBee Radio and new architecture.
'                Removed programming and passthru logic.
'
'
' *****************************************************************************/
#pragma config  RETEN=OFF, SOSCSEL=LOW, XINST=OFF, FOSC=HS1, PLLCFG=ON,WDTEN=OFF
//ram int                   MS;
ram char                  WorkBuffer[74];

// SeeTron OLED Commands
ram char Clear[2]  = {12};
ram char BackOn[2] = {14};



#include    "stdio.h"
#include    <usart.h>
#include    <timers.h>
#include    "cranwellP18F25K80.h"
#define MAXNODES     25
#define iACK         7
#define iNAK         8
#define OK           1
#define NG           -1
#define MessageSize  255   
#define ReplyLength  47


// Fix for bad strncpy
char  * strncpy(char *s1, const char *s2, size_t n);
int    CheckDigit(char Number[]);
void   BumpSendMessageNumber(int Unit);
void   ResetLCD(void);
void   SendLCD(char Msg[], int Len);
void   DisplayLCD(void);
int    LCDGetCommand();
void   ClearLine4(void);
int    Transaction(int Unit, char Command[],char Port[], int retries, int TOT);
void   Broadcast(char Data[]);
void   InitializeData(void);
char   Keyin(void);
void   GlobalReset(void);
void   ShowStatus(void);
void   ArmUnits(void);
void   DisArmUnits(void);
void   Trace(char Msg[], int Direction);
void   ReadyShow(void);
int    StartSession(int Unit);
int    GetPedalStatus(void);
void   InitializeHardware(void);
char   GetKeypad(void);
int    StartUnits(void);
void   Blink(int);
void   BlinkOK(void);
void   BlinkNG(void);
void   LoadShow(char ShowNumber[]);
void   ResetLEDs(void);
void   FireCue(int Cue);
int    CheckTimer1(void);
int    GetStatus(int,int);
int    GetCommand(void);
void   WriteOLED(char Msg[]);
void   BroadcastTime(void);
void   Scroll(void);
void   FastPoll(void);
void   SlowPoll(void);
int      GetTriggerCommand(void);
void   GetNextUnitAndPort(void);
void   FirePort(void);
void   TriggerPort(void);
void   TriggerCue(void);
int    GetSubcmd(void);


#pragma udata BIGDATA
#pragma idata IDATA

int LCDptr = 0;

struct StatusStruct
{
   char Addr[4];
   char VAddr[4];
   char Port[17];
   char Show[2];
   char Armed[2];
   char Voltage[5];
};


struct PeerData
{
   int  iNextRecMessageNumber;
   int  iNextSendMessageNumber;
   int  Active;
   int   SlowPollDelay;
   struct StatusStruct Status;
};


//==================================================================================================


ram char             Line2[21];
ram char             RecResult;
ram int              intCode;
ram int              intPort;
ram int              intData;
ram int              iCheck;
ram char             GlobalResetData[] = "1019999999900000000";       // Global Reset
ram char             TimeMsg[]         = "1019999800000000000";       // Time 0 
ram char             GlobalLoadShow[]  = "1019990800100000000";       // Load Show 1
ram char             GlobalArmMsg[]    = "1019991099900000000";
ram char             GlobalDisarmMsg[] = "1019991199900000000";
ram char             GlobalFireCueMsg[]= "1019994400000000000";
ram char             FastPollMsg[]        = "10199995999010110210310410510610710810910";     // specify 10 Units per bank
ram char             SlowPollMsg[]        = "10199994999010110210310410510610710810910";
ram char             SlowPollDelayMsg[]   = "10199993999";
ram unsigned char    i,j;
ram char             cdigit[2] = "\0";
ram char             Reset[]             = "99";
ram char             Command[2]          = "";
ram char             Fire[]              = "04";
ram char             Arm[]               = "10";
ram char             Disarm[]            = "11";
ram char             Sense[]             = "05";
ram char             Code[3]             = "";
ram int              iCode;
ram char             Unit[3]             = "";
ram char             Port[3]             = "";  
ram char             cCheck[2]           = ""; 
ram int              iUnit;
ram int              Retry;
ram char             LCDMsg[21]           = "";
ram char             CR[]               ="\r";
ram char             *cp;
ram char             cReceivedMessageNumber[2];
ram char             cNextSendMessageNumber[2];
ram int              RC;
ram char             OneWorkUnit[] = "01";
int                  iCmd = 0;
int                  iSubCmd = 0;
ram char             Zeroes[] = "00000000000000";
ram char             TimeZero[] = "00000000";
ram char             Start[] = "01";
ram char             FirstUnit[] = "02";
ram unsigned char    *p;
ram char             Radioinstring[] = "<== ";
ram char             Radiooutstring[] = "==>"; 
ram char             TraceMsg[73];
ram char             CRLF[] ="\r\n";
ram int              TRC = FALSE;
ram char             WorkMsg[22];
ram char             Port0[] = "00";
ram char             PianoStat[] = "Piano";
ram char             NoPianoStat[] = "No Piano";
ram char             StartStat[] = "Start";
ram char             OKStat[] = "OK";
ram char             NGStat[] = "NG";
ram char             CLS[] = {27,'1'};
ram char             ErrorMsg[] = "Error";
ram char             One[] = " 1";
ram char             Two[] = " 2";
ram char             Three[] = " 3";
ram char             ArmMsg[] = "Ready to Arm";
ram char             CharOne[] = "1";
ram char             CharTwo[] = "2";
ram int              UnitIndex;
ram int            SlowPollTOT;


//LCD and Keypad

unsigned char        Key, Key2;
unsigned char        k0[] = "0";
unsigned char        k1[] = "1";
unsigned char        k2[] = "2";
unsigned char        k3[] = "3";
unsigned char        k4[] = "4";
unsigned char        k5[] = "5";
unsigned char        k6[] = "6";
unsigned char        k7[] = "7";
unsigned char        k8[] = "8";
unsigned char        k9[] = "9";
unsigned char        k10[] = "*";
unsigned char        k11[] = "#";

ram char             PedalError[] = "Plug in Pedal";
ram char             OKc[] = "OK";
ram char             NGc[] = "NG";
ram char             AckMsg[] = "ACK";
ram char             NakMsg[] = "Nak";
ram struct           PeerData UnitData[MAXNODES];
ram char             FullMsg[MessageSize+2];   // Add PC Transaction type to byte 1 Null termination characters
ram char             Blank[22]= "                     " ;
ram unsigned char    Serial = FALSE; 
ram char             RadioReply[ReplyLength+1];
ram char             Status[39];
ram unsigned char    Piano = FALSE;
ram int              Event = 0;
ram unsigned char    Pedal;
ram char             ACK[] = "7";
ram unsigned char    Armed = FALSE;
ram char             Ascii[4];
ram int              NumEvents;
ram int              NumCues;
ram int              n;
ram unsigned char    PORTBValue;
ram int              ROW;
ram int              COL;
ram char             Keypad[4][4] = {'1','4','7','*','2','5','8','0','3','6','9','#','A','B','C','D'};
ram char             CHAR;
ram int              KbdReset = 0;
ram char             Msg[3] = "";
ram int              HighestUnit;
ram char             KbdChar;
ram char             KChar;
ram char             ShowID;
ram char             Show1[]       = "001";
ram char             Show2[]       = "002";
ram char             Show3[]       = "003";
ram char             ToLoad[]      = "001"; 
ram char             StartingMsg[] = "Ver 02/13/22   ";
ram char             Offline[]     = "No Units Active";
ram char             CommandMsg[]  = "Cmd L|A|S|T|D ?";
ram char             LoadShowMsg[] = "Loading Show";
ram char             ReadyingMsg[] = "Scanning Units";
ram char             ShowErrMsg[]  = "Show Load Err";
ram char             ShowOKMsg[]   = "Show Loaded OK";
ram char             StatusErrMsg[]= "Err get Status";
ram char             StatusOKMsg[] = "Got Unit status";
ram char             NoShowMsg[]   = "Show not loaded";
ram char             Work[3] = "";
ram char             Work2[20] = "";
ram unsigned int     Secs = 0;
ram int              CueFired = 0;
ram int              div;
ram char             str1[10];
ram char             str2[10];
ram int            UnitToFire;
ram int            PortToFire; 


void main(void)
{



   TRC = TRUE;

   InitializeHardware();
   DelayMS(2000);

   LCDClear();
   WriteOLED(StartingMsg);
   DelayMS(5000);
   LCDClear();

   GlobalReset();

   WriteOLED(ReadyingMsg);

   // Poll Network
   DelayMS(1000);


   // Allow 5 resties to start network.

   for ( i=0; i<5; i++ )
   {
      FastPoll();
      if ( UnitIndex == 0 )
      {
         GlobalReset();
      }
      else
      {
         SlowPoll();
         i=5;
      }

   }





   // Show Highest working unit
   if ( UnitIndex > 0 )
   {
      LCDClear();
      sprintf(WorkMsg,(const far rom char*) "%d Units Active",UnitIndex);
      WriteOLED(WorkMsg);
      DelayMS(1000);  
      StartUnits();


   }

   else
   {
      BlinkNG();
      LCDClear();
      WriteOLED(Offline);
      // Infinite loop until user RESETs.
      while ( 1 );

   }

   // Show Polling Results
   Scroll();




   // Read State of Piano Footswitch (Pickle switch)
   // Piano switch is Normally Open jack with 10K pullup resistor. = Logical 1
   // With switch plugged in, the pin is grounded = Logical 0
   // When switched is pressed, pin goes to Logical 1

   Piano = !GetPedalStatus();
   if ( Piano )
   {
      // If Piano pedal or Pickle switch connected
      // Load Show #1
      DelayMS(500);
      LCDClear();
      WriteOLED(LoadShowMsg);
      LoadShow(Show1);

      // Wait 5 seconds for units to load show
      DelayMS(5000);


   }




   while ( TRUE )
   {
      if ( Piano )      // If Piano Switch Plugged In
      {

         iCmd = 0;
         if ( Armed == FALSE )
         {
            while ( iCmd != 2 )
            {
               LCDClear();
               WriteOLED(ArmMsg);
               SendLCD(CRLF,2);
               iCmd = GetCommand();
               StartTimer1();
            } 
            ArmUnits();
         }

         Event++;
         LCDClear();
         memset(Line2,'\0',20);
         sprintf(Line2,(const far rom char*) "CUE  0%u",Event);
         SendLCD(Line2,20);

         // Wait for Pedal Down
         while ( !GetPedalStatus() )
         {
            if ( CheckTimer1() == 1 )
            {
               Secs ++;
               div = Secs / 10;

               if ( (div * 10) == Secs )
               {
                  BroadcastTime();
               }
            }

         }


         FireCue(Event); 
         // Wait foir Pedal Up
         while ( GetPedalStatus() );

      }
      else
      {

         // Piano Switch Not Plugged in
         LCDClear();
         WriteOLED(CommandMsg);
         iCmd = GetCommand();


         switch ( iCmd )
         {
            case 1:
               // Load Show
               LCDClear();
               sprintf(WorkMsg,(far const rom char *) "Loading Show %s",ToLoad);
               WriteOLED(WorkMsg);  
               LoadShow(ToLoad);
               DelayMS(5000);
               LCDClear();
               sprintf(WorkMsg,(far const rom char *) "Show Loaded");
               WriteOLED(WorkMsg);
               DelayMS(1000); 
               break;
            case 2:
               // Arm Units
               ArmUnits();
               break;
            case 3:
               // Disarm Units
               DisArmUnits();

               break;
            case 4:
               // Get Status 
               HighestUnit = UnitIndex;
               Scroll();

               break;

            case 5:
               // Trigger 

               LCDClear();
               sprintf(WorkMsg,(far const rom char *) "(P)ort / (C)ue ?");
               WriteOLED(WorkMsg);
               DelayMS(250); 
               iSubCmd = GetSubcmd();
               switch ( iSubCmd )
               {
                  // Trigger Port
                  case 4:
                     TriggerPort();
                     break;

                     // Trigger Cue
                  case 2:
                     TriggerCue();
                     break;

                  default:
                     sprintf(WorkMsg,(far const rom char *) "Bad Response");
                     WriteOLED(WorkMsg);
                     DelayMS(1000);    
                     break;                  
               }



               break;



         }    // End switch

      }
   }   // end While True

}   // End Main


char IntToCh(int value)
{
   char atab[11] = "0123456789";
   return atab[value];
}

void BumpSendMessageNumber(int Unit)
{
   ram int ii;


   //Trace(BumpMsg,None);
   if ( UnitData[Unit].iNextSendMessageNumber ==9 )
   {
      UnitData[Unit].iNextSendMessageNumber = 0;     // (it will be incremented to 1)
   }
   UnitData[Unit].iNextSendMessageNumber += 1;

   ii = UnitData[Unit].iNextSendMessageNumber;


}

int    Transaction(int UnitIndex, char Command[],char Port[], int retries, int TOT)
{


   ram int Len;

   memset(FullMsg,'\0',sizeof(FullMsg));
   memset(cNextSendMessageNumber,'\0',sizeof(cNextSendMessageNumber));

   // Add Message Number 
   sprintf(FullMsg,(const far rom char*) "%1d%s%03s%s%s%s",UnitData[UnitIndex].iNextSendMessageNumber, OneWorkUnit,UnitData[UnitIndex].Status.Addr,Command,Port,TimeZero);
   cdigit[0] = IntToCh(CheckDigit(FullMsg));
   strcat(FullMsg,cdigit); 

   for ( Retry = 0; Retry < retries; Retry++ )
   {
      Len = strlen(FullMsg);
      // Send Message
      Pin15HIGH();
      vputs1USART(FullMsg,Len);
      DelayMS(1);
      Pin15LOW();
      memset(RadioReply,'\0',ReplyLength+1);


      RC = vgets1USART(RadioReply,TOT);
      if ( RC >0 )
      {
         memset(Code,'\0',3);
         strncpy(Code,RadioReply+6,2);
         intCode = atoi(Code);

         if ( intCode == iACK )
         {


            // Extract Status Data
            //ShowStatus();

            BumpSendMessageNumber(UnitIndex);

            Pin13HIGH(); 
            return(OK);
         }
         if ( intCode == iNAK )
         {


         }

      }

      DelayMS(100);

   }      

   Pin14HIGH();
   Armed = FALSE;
   //LCDOFF();


   return NG;
}

void    Broadcast(char Data[])
{
   ram int Len;

   Len = strlen(Data);
   Pin15HIGH();  
   vputs1USART(Data,Len);
   DelayMS(2);
   Pin15LOW();
   return;

}



int CheckDigit(char Msg[])
{

   // calculate check digit

   ram int sum, i,d,c,cd;
   ram int IntTemp;
   ram char cTemp[2];
   ram int factor[] = {3,7,1};

   cd = 0;
   memset(cTemp,'\0',2);
   i=0;

   for ( c=0; c< strlen(Msg); c++ )
   {
      if ( i>2 ) i=0;
      d = factor[i];
      strncpy(cTemp,Msg+c,1);
      cd = cd + d * atoi(cTemp);
      i++;

   }

   // calculate remainder of div by 10
   IntTemp = cd/10;
   IntTemp = cd-(IntTemp * 10);
   return(abs(IntTemp));

}

void InitializeData(void)
{
   memset(cReceivedMessageNumber,'\0',2);
   memset(Status,'\0',sizeof(Status));
   memset(Status,'0',sizeof(Status)-1);
   memset(cCheck,'\0',sizeof(cCheck));

   for ( i=0; i<MAXNODES; i++ )
   {
      UnitData[i].iNextRecMessageNumber = 1;
      UnitData[i].iNextSendMessageNumber=1;
      UnitData[i].Active = 0;
   }

}



void SendLCD(char Msg[], int Len)
{
   Myputs2USART( Msg,Len);
}



int GetCommand(void)
{

   // Get Command Start
   KChar = GetKeypad();
   SendLCD(CLS,2);

   if ( KChar != 'A' ) return -1;

   iCmd = 0;
   KChar = GetKeypad();
   if ( KChar == '5' ) iCmd = 1;
   if ( KChar == '2' ) iCmd = 2;
   if ( KChar == '3' ) iCmd = 3;
   if ( KChar == '7' ) iCmd = 4;
   if ( KChar == '8' ) iCmd = 5;
   if ( KChar == 'C' ) iCmd = 6;
   if ( KChar == '*' ) iCmd = 7;
   if ( KChar == '#' ) iCmd = 8;


   if ( iCmd == 0 )
   {
      SendLCD(CLS,2);
      SendLCD(ErrorMsg,5);
      return -1;
   }

   switch ( iCmd )
   {
      case 1:
         sprintf(LCDMsg,(const far rom char*) "Load Show");
         break;
      case 2:
         sprintf(LCDMsg,(const far rom char*) "Arm All");
         break; 
      case 3:
         sprintf(LCDMsg,(const far rom char*) "Disarm All");
         break;
      case 4:
         sprintf(LCDMsg,(const far rom char*) "Get Status");
         break;
      case 5:
         sprintf(LCDMsg,(const far rom char*) "Start Show");
         break;
      case 6:
         LCDClear();
         return -1;
         break;
   }

   WriteOLED(LCDMsg);

   if ( iCmd == 1 )
   {
      SendLCD(CRLF,2);
      sprintf(LCDMsg,(const far rom char*) "Enter Show #");
      WriteOLED(LCDMsg);
      ShowID = GetKeypad();
      if ( (ShowID == '1')  | (ShowID == '2') | (ShowID == '3') )
      {
         switch ( ShowID )
         {
            case '1':
               SendLCD(One,2);
               strcpy(ToLoad,Show1);
               return 1;

            case '2':
               SendLCD(Two,2);
               strcpy(ToLoad,Show2);
               return 1;

            case '3':
               SendLCD(Three,2);
               strcpy(ToLoad,Show3);
               return 1;

         } 


      }
      else
      {
         SendLCD(CLS,2);
         SendLCD(ErrorMsg,5);
         return -1;
      }
   }

   KChar = GetKeypad();
   if ( KChar != 'B' ) return -1;

   return iCmd;  

}






void BroadcastTime(void)
{

   ram int Len;


   if ( Armed == TRUE )
   {
      memset(FullMsg,'\0',sizeof(FullMsg));
      if ( CueFired == 0 ) Secs = 0;
      sprintf(FullMsg,(const far rom char*) "1019999800%05d000",Secs);
      cdigit[0] = IntToCh(CheckDigit(FullMsg));
      strcat(FullMsg,cdigit); 
      Broadcast(FullMsg);

   }

}



void ArmUnits(void)
{

   memset(FullMsg,'\0',sizeof(FullMsg));
   strcpy(FullMsg,GlobalArmMsg);
   cdigit[0] = IntToCh(CheckDigit(FullMsg));
   strcat(FullMsg,cdigit); 
   Broadcast(FullMsg);
   Armed = TRUE;
   StartTimer1();
}

void DisArmUnits(void)
{
   memset(FullMsg,'\0',sizeof(FullMsg));
   strcpy(FullMsg,GlobalDisarmMsg);
   cdigit[0] = IntToCh(CheckDigit(FullMsg));
   strcat(FullMsg,cdigit); 
   Broadcast(FullMsg);
   Armed = FALSE;
}

int GetPedalStatus(void)
{


   GetPin26();
   return GetPin26();;
}

void InitializeHardware(void)
{

   // OK
   ram int i;

   Open2USARTpic18f25k80();      // Open LCD Port at 9600 BPS
   Open1USARTpic18f25k80();      // Open Radio Port
   ANCON1 = 0x00;                // All digital keypad inputs
   LCDReset();
   ResetLEDs();
   return;
}

void BlinkOK(void)
{
   // OK

   Pin13HIGH();
   return;
}

void BlinkNG(void)
{
   // OK

   Pin14HIGH();
   return;
}



void Blink(int Number)
{

   // OK
   Pin14LOW();
   for ( i=0; i<Number; i++ )
   {
      Pin25HIGH();
      DelayMS(1000);
      Pin25LOW();
      DelayMS(1000);
   } 
   return;
}



void LoadShow(char ShowNumber[])
{
   memset(FullMsg,'\0',sizeof(FullMsg));
   strcpy(FullMsg,GlobalLoadShow);
   memcpy(FullMsg+8,(void *)ShowNumber,3);
   cdigit[0] = IntToCh(CheckDigit(FullMsg));
   strcat(FullMsg,cdigit); 
   Broadcast(FullMsg);

} 

void GlobalReset(void)
{
   // OK

   ram int Len;

   InitializeData();

   memset(FullMsg,'\0',sizeof(FullMsg));
   strcpy(FullMsg,GlobalResetData);

   // Add Check Code

   cdigit[0] = IntToCh(CheckDigit(FullMsg));
   strcat(FullMsg,cdigit); 
   Broadcast(FullMsg);
   ResetLEDs();
   DelayMS(100);

}

void FastPoll(void)
{
   //  Allow 10 units per bank for max 10 banks (Total = 100 units)
   //
   //  The FastPoll Message tells the field modules that the following Field Modules may be active.
   //
   //
   //                                  Uints 1-10
   //                                  Units 101-110
   //                                  Units 201-210
   //                                  Units 301-310
   //                                  Units 401-410
   //                                  Units 501-510
   //                                  Units 601-610
   //                                  Units 701-710
   //                                  Units 801-810
   //                                  Units 901-910
   //
   // Allocate 2 second timeout on Read

   ram int Len;
   ram   int   ComputedDelay = 50;

   memset(FullMsg,'\0',sizeof(FullMsg));
   strcpy(FullMsg,FastPollMsg);

   // Add Check Code

   cdigit[0] = IntToCh(CheckDigit(FullMsg));
   strcat(FullMsg,cdigit); 
   Broadcast(FullMsg);

   // Read Replies until Timeout (No more responding units.  2 Seconds allows 10 units per bank.)

   UnitIndex = 0;

   while ( 1 )
   {

      RC = vgets1USART(RadioReply,2000);
      if ( RC >0 )
      {
         memset(Code,'\0',3);
         strncpy(Code,RadioReply+6,2);
         intCode = atoi(Code);

         if ( intCode == iACK )
         {

            // Assume al units are 16 port units so dont examine ACK data
            // Retrieve Address, Virtual Adress, Show Loaded, Voltage

            memset(UnitData[UnitIndex].Status.Addr,'\0',sizeof(UnitData[UnitIndex].Status.Addr));
            //memcpy(UnitData[UnitIndex].Status.Addr,(void *)RadioReply+17,3);
            memcpy(UnitData[UnitIndex].Status.Addr,(void *)RadioReply+15,3);
            memset(UnitData[UnitIndex].Status.VAddr,'\0',sizeof(UnitData[UnitIndex].Status.VAddr));
            //memcpy(UnitData[UnitIndex].Status.VAddr,(void *)RadioReply+20,3);
            memcpy(UnitData[UnitIndex].Status.VAddr,(void *)RadioReply+18,3);
            memset(UnitData[UnitIndex].Status.Show,'\0',sizeof(UnitData[UnitIndex].Status.Show));
            memcpy(UnitData[UnitIndex].Status.Show,(void *)RadioReply+12,1);
            memset(UnitData[UnitIndex].Status.Voltage,'\0',sizeof(UnitData[UnitIndex].Status.Voltage));
           // memcpy(UnitData[UnitIndex].Status.Voltage,(void *)RadioReply+13,4);
            memcpy(UnitData[UnitIndex].Status.Voltage,(void *)RadioReply+13,2);
            UnitData[UnitIndex].SlowPollDelay = ComputedDelay;
            ComputedDelay += 70;
            UnitIndex ++;

         }
      }
      if ( RC < 1 )
      {
         return;
      }


   }
}

void SlowPoll(void)
{

   ram int    Len;
   ram   char  WorkMessage[10] ;

   // Build a Slow Poll Delay message.  Unit delays were calculated on Fast Poll.
   memset(FullMsg,'\0',sizeof(FullMsg));
   memset(WorkMessage,'\0',sizeof(WorkMessage));
   strcpy(FullMsg,SlowPollDelayMsg);
   sprintf(WorkMsg,"%02d",UnitIndex);
   strncat(FullMsg,WorkMsg,2);
   // Build a Slow Poll Delay message.  Unit delays were calculated on Fast Poll.
   for ( i=0; i<UnitIndex; i++ )
   {
      sprintf(WorkMessage,"%3d%05d",atoi(UnitData[i].Status.Addr), UnitData[i].SlowPollDelay);
      strncat(FullMsg,WorkMessage,8);
      SlowPollTOT = UnitData[i].SlowPollDelay + 200;     // Extra Time added
   }

   // Add Check Code

   cdigit[0] = IntToCh(CheckDigit(FullMsg));
   strcat(FullMsg,cdigit); 
   Broadcast(FullMsg);
   DelayMS(50);

   // Build A SlowPoll Message

   memset(FullMsg,'\0',sizeof(FullMsg));
   memset(WorkMessage,'\0',sizeof(WorkMessage));
   strcpy(FullMsg,SlowPollMsg);
   // Add Check Code

   cdigit[0] = IntToCh(CheckDigit(FullMsg));
   strcat(FullMsg,cdigit); 
   Broadcast(FullMsg);
   DelayMS(50);


   UnitIndex = 0;
   while ( 1 )
   {

      RC = vgets1USART(RadioReply,SlowPollTOT);
      if ( RC >0 )
      {
         memset(Code,'\0',3);
         strncpy(Code,RadioReply+6,2);
         intCode = atoi(Code);

         if ( intCode == iACK )
         {

            // Retrieve Port Status from Slow Poll Reply
            memset(UnitData[UnitIndex].Status.Port,'\0',sizeof(UnitData[UnitIndex].Status.Port));
            memcpy(UnitData[UnitIndex].Status.Port,(void *)RadioReply+11,16);
            UnitIndex ++;

         }
      }
      if ( RC < 1 )
      {
         return;
      }


   }


}



void ResetLEDs(void)
{
   // OK

   Pin13LOW();
   Pin14LOW();
   Pin16LOW();
   Pin28LOW();
}

int StartSession(int UnitIndex)
{
   // OK

   int retries = 2;
   int TOT = 500;

   ResetLEDs();
   RC = NG;

   DelayMS(TOT);
   RC = Transaction(UnitIndex,Start,Port0,retries,TOT);
   if ( RC == OK )
   {
      UnitData[UnitIndex].Active = 1;

   }
   else
   {
      UnitData[UnitIndex].Active = 0;
   }
   return RC;

}

int StartUnits(void)
{

   for ( i=0; i<UnitIndex; i++ )
   {
      RC = StartSession(i);  
      if ( RC < 0 )
      {
         break;
      }
   }
   return HighestUnit;
}


char GetKeypad(void)
{

   // OK

   // Set Input and Output Ports

   TRISA = TRISA & 0xf0;
   TRISB = TRISB | 0x0f;
   LATA = 0;

   // wait for Key Up
   LATA = LATA & 0xf0;
   // Wait for debounce hardware
   DelayMS(20);
   PORTBValue = PORTB & 0x0f;

   while ( PORTBValue == 0x0f )
   {
      PORTBValue = PORTB & 0x0f;
      if ( CheckTimer1() == 1 )
      {
         Secs ++;
         div = Secs / 10;
         if ( (div * 10) == Secs )
         {
            BroadcastTime();
         }
      }
   }

   DelayMS(20);    // Wait for debounce hardware

   LCDClear();

   switch ( PORTBValue )
   {
      // Col 1
      case 0x07:
         COL = 0; 
         break;
         // Col 2
      case 0x0b:
         COL = 1;
         break;
         // Col 3
      case 0x0d:
         COL = 2;
         break; 

         // Col 4
      case 0x0e:
         COL=3;
         break;
   } 

   // Row 1 

   LATA = 0x0E;
   DelayMS(20);      // Wait for debounce hardware
   PORTBValue = PORTB & 0x0f;
   if ( PORTBValue < 0x0f )
   {
      ROW = 0;

   }


   // Row 2
   LATA = 0x0D; 
   DelayMS(20);     // Wait for debounce hardware
   PORTBValue = PORTB & 0x0f;
   if ( PORTBValue < 0x0f )
   {
      ROW=1;

   }


   // Row 3
   LATA = 0x0B;
   DelayMS(20);     // Wait for debounce hardware
   PORTBValue = PORTB & 0x0f;
   if ( PORTBValue < 0x0f )
   {
      ROW=2;

   }


   // Row 4
   LATA = 0x07;
   DelayMS(20);     // Wait for debounce hardware
   PORTBValue = PORTB & 0x0f;
   if ( PORTBValue < 0x0f )
   {
      ROW=3;

   }

   KbdChar = Keypad[COL][ROW]; 


   // wait for Key Up
   LATA = LATA & 0xf0;
   DelayMS(20);     // Wait for debounce hardware


   PORTBValue = PORTB & 0x0f;

   while ( PORTBValue < 0x0f )
   {
      PORTBValue = PORTB & 0x0f;
   } 

   LCDON();
   return KbdChar;

}


void FireCue(int Cue)
{
   memset(Work2,'\0',sizeof(Work2));
   sprintf(Work,(const far rom char*) "%03d",Cue);
   memcpy(Work2,(void *)GlobalFireCueMsg,sizeof(GlobalFireCueMsg));
   memcpy(Work2+8,(void *)Work,3);
   cdigit[0] = IntToCh(CheckDigit(Work2));
   strcat(Work2,cdigit);
   Broadcast(Work2); 
   Secs = 0;

   // Set Timer for 1 Sec
   TMR1H = 0x7F;
   TMR1L = 0xFF;
   TimerTick = 0;

   CueFired = 1;

}

int CheckTimer1(void)
{
   if ( TimerTick == 1 )
   {
      TimerTick = 0;
      return 1;
   }
   return 0;
}    



void WriteOLED(char Msg[])
{

   Myputs2USART( Msg,strlen(Msg));

}





void Scroll(void)
{

   ram  int iUnit;
   char KChar = '*';
   char StatStr[8];
   char Show[] = "Show ";
   char NoShow[] = "NoSh ";
   char ArmedStat[] = "A ";
   char DisarmedStat[] = "D ";
   int  FirstTime = 1;

   iUnit = 0;

   LCDClear();

   while ( 1 )
   {
      if ( FirstTime == 0 )
      {
         KChar = GetKeypad();
      }
      FirstTime = 0;


      switch ( KChar )
      {
         case '*':
            // Scroll Down
            iUnit -= 1;
            if ( iUnit < 0 ) iUnit = 0;
            break;
         case '#':
            // Scroll Up
            iUnit += 1;
            if ( iUnit > UnitIndex-1 ) iUnit = UnitIndex - 1;
            break;
         default:
            // Exit Scrolling
            return;
      }
      LCDClear();
      memset(WorkMsg,'\0',sizeof(WorkMsg));
      memset(StatStr,'\0',sizeof(StatStr));

/*
      if ( !strncmp(UnitData[iUnit].Status.Armed ,k1,1) )
      {
         strcpy(StatStr,ArmedStat);
      }
      else
      {
         strcpy(StatStr,DisarmedStat);
      }

      if ( strncmp(UnitData[iUnit].Status.Show ,k0,1) )
      {
         // Show Loaded 	
         strcat(StatStr,Show);
         sprintf(WorkMsg,(const far rom char *) "U%s %s%s%s",UnitData[iUnit].Status.Addr,StatStr,UnitData[iUnit].Status.Show,CRLF);

      }
      else
      {
         // No Show Loaded
         strcat(StatStr,NoShow);
         sprintf(WorkMsg,(const far rom char *) "U%s %s%s",UnitData[iUnit].Status.Addr,StatStr,CRLF);

      }
*/

	  strcpy(StatStr,UnitData[iUnit].Status.Voltage);
	  sprintf(WorkMsg,(const far rom char *) "U%s %s%s",UnitData[iUnit].Status.Addr,StatStr,CRLF);

      WriteOLED(WorkMsg);

      memset(WorkMsg,'\0',sizeof(WorkMsg));
      memcpy((void *)WorkMsg,(void *)UnitData[iUnit].Status.Port,16);
      WriteOLED(WorkMsg);           

   }
   return ;
}



void  GetNextUnitAndPort(void)
{
   if ( UnitToFire == 0 )
   {
      UnitIndex = 0;
      // First time
      PortToFire = 0;
   }

   PortToFire += 1;

   if ( PortToFire > 16 )
   {
      PortToFire = 1;
      UnitIndex += 1;
      //UnitToFire = UnitIndex;
   }
   UnitToFire = atoi(UnitData[UnitIndex].Status.Addr);

}


int GetTriggerCommand(void)
{
   char KChar;
   int   Digit = 0;
   int Port;



   while ( Digit < 2 )
   {
      // Get Command Start
      KChar = GetKeypad();
      if ( KChar == '#' )
      {

         return 0;
      }
      if ( KChar < '0'  || KChar > '9' )
      {
         continue;
      }

      Port = KChar - 48;
      if ( Digit == 0 )
      {
         PortToFire = Port * 10;
      }
      else
      {
         PortToFire += (Port -1);
      }

      if ( PortToFire > 16 )
      {
         UnitToFire = PortToFire / 16;
         PortToFire = PortToFire - (UnitToFire * 16);
      }
      Digit++;
   }
   return -1;

}


void  FirePort(void)
{
   char Port[4];
   char Fire[3] = "04";

   sprintf(Port,"%03d",PortToFire);

   Transaction(UnitIndex,Fire,Port,2,100);


}

void TriggerPort(void)
{


   while ( 1 )
   {

      UnitToFire = 0;
      PortToFire = 0;

      while ( 1 )
      {

         // Calc Unit and Port to fire

         GetNextUnitAndPort();

         LCDClear();
         memset(Line2,'\0',20);
         sprintf(Line2,(const far rom char*) "U %03d P %02d",UnitToFire,PortToFire);
         SendLCD(Line2,20);

         if ( GetTriggerCommand() == 0 )
         {
            FirePort();
         }



      }

   }
}




void TriggerCue(void)
{

   Event = 1;
   while ( 1 )
   {
      LCDClear();
      memset(Line2,'\0',20);
      sprintf(Line2,(const far rom char*) "Cue %02d",Event);
      SendLCD(Line2,20);

      if ( GetTriggerCommand() == 0 )
      {

         FireCue(Event);
         Event ++;
      }

   }


}

int GetSubcmd(void)
{
   int iSubCmd;


   //iCmd = 0;
   KChar = GetKeypad();
   if ( KChar == '5' ) iSubCmd = 1;
   if ( KChar == '2' ) iSubCmd = 2;
   if ( KChar == '3' ) iSubCmd = 3;
   if ( KChar == '7' ) iSubCmd = 4;
   if ( KChar == '8' ) iSubCmd = 5;
   if ( KChar == 'C' ) iSubCmd = 6;
   if ( KChar == '*' ) iSubCmd = 7;
   if ( KChar == '#' ) iSubCmd = 8;


   return iSubCmd;
}








