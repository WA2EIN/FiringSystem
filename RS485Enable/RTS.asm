
;******************************************************************************
;  File Name    : RTS  RS-485 Transmit Enable
;  Version      : 1.0
;  Description  : Generates RTS signal for RS-485 from RS-232 Data line.
;  Author       : Peter Cranwell
;  Target       : Microchip PIC 12F683 Microcontroller
;  Compiler     : Microchip Assembler (MPASM)
;  IDE          : Microchip MPLAB IDE v8.00
;  Programmer   : PICKSTART programmer
;  Last Updated : 4/26/11
; *******************************************************************
;
; RS/232 - RS-485  Transmit Data Enable
;     (C) 2011 P. Cranwell
;
;     Port Usage
;
;     GP0    Date Input         Pin 7
;     GP1    Data Output        Pin 6
;     GP2    Data Enable        Pin 5
;
;     NOTE:  TTL levels are inverted from RS-232 levels.  A TTL level Start Pulse is a SPACE = 0 Level.
;            A TTL Level STOP bit ia a Mark = + Level. 
;
;
;   The device repeats the signal level on the input pin & propagates it to the output pin.
;
;   Upon receipt of a 0 level (Start bit or data bit) the Transmit Enable output is raised and the timer is turned on.
;   The transmit enable is raised 8 microseconds before the data is repeated (at a clock speed of 20MHZ).  This is 1/2 bit time at 56KBPS.
;
;
;   Timer1 ioperates at FOSC/4 = @5MHZ or .2 microseconds per timer count.
; 
;   1 Byte time @ 38400 BPS =  260 Microseconds 
;     
;   1 Byte time @ 9600 BPS = 1041 Microseconds
;      Time to drop DE = 1 Byte time Timer Count.  = X'1455'   (FFFF - 1455 = E6AA)
;
;
;
;***********************************************************************
Bank0  macro
       bcf    STATUS,RP0 
       endm

Bank1  macro
       bsf     STATUS,RP0
       endm


#include <p12F683.inc>
    ; Compiler configuration bits set to override the config setting.
    ; The hardware is using a 20 MHZ crystal.
    ; Data definitions follow
     cblock 0x20
TimerLow                ; Define two 8-bit timer values
TimerHigh  
     endc

     org 0                  ; Main Loop
     goto   Start 


; Interrupts servied here      
     org 4                  ; Interrupt Vector
     Bank1
     ; check to see what type of interrupe.  Either Port change or Timer
     btfss  INTCON,GPIF     ; skip if IO Interrupt
     goto   TimerPop


     ; I/O interrupt logic    
    
     ; get GP0 latch value into W Reg
     Bank0
     
     btfsc   GPIO,GP0
     goto Mark 
    
     ; SPACE Logic is entered on Space and Start Bit = 0 Level.

     ; Bank0 already selected
     bsf    GPIO,GP2             ; Enable RS-485 Transmit & RS-232 RTS
     
     ; initialize timer Count for 9600 BPS.  
     ; This will work at higher speeds if peer uses slight delay before responding.
     ;
     movlw    0xE6      
     movwf    TMR1H
     movlw    0xAA
     movwf    TMR1L 
     ; Enable Timer
     movlw    0x05     
     movwf    T1CON
    

     goto   MarkAndSpace
      


Mark:
     ; Entered on Space and Stop Bit = + Level
     ; Keep Mark and Space pathlengths the same
     nop
     nop
    
  

MarkAndSpace:
     ; Echo Input Latch data to Output Latch at GP0
     Bank0
     btfss   GPIO,GP0
     goto    SetLow
     bsf     GPIO,GP1
     goto Done
SetLow
     bcf     GPIO,GP1
Done:
     


     Bank1
     bcf    INTCON,GPIF          ; Clear interrupt

     retfie

TimerPop:
     ; bank 1 already selected
     bcf      INTCON,GPIF
     Bank0
     bcf      GPIO,GP2         ; Drop RTS
     bcf      T1CON,TMR1ON     ; Turn Off Timer
     bcf      PIR1,TMR1IF
     retfie
     



    org     0x100
Start:
    ;Setup chip options, enable timer and IOC interrupts and then sleep until interrupted.
      
    Bank0
   
    movlw    0x07
    movwf    CMCON0          ; Turn Comparators off
   
    movlw    0xC8            ; GIE=1, PEIE=1, GPIE=1, GPIF=0
    movwf    INTCON

    Bank1
    movlw    0x00            ; GP0 is input, all other are output
    movwf    TRISIO  
    clrf     ANSEL           ; Make all ports as digital I/O
    

    Bank0
    movlw    0x03            ; Initialize Input & Output to +, Transmit Enable to 0
    movwf    GPIO

    
    Bank1
    movlw    0x01            ; GP0 is input, all other are output
    movwf    TRISIO  
        
    bsf      IOC,IOC0        ; GP0 Interrupt on Change
    bcf      T1CON,TMR1ON    ; Turn Off Timer
    bsf      PIE1,TMR1IE     ; Enable Timer interrupts

   

   
   
Loopforever:
     ; the following is just a test
     movwf 0x02
     goto     Loopforever
    end; EOF: RTS