#pragma config  OSC=INTIO67,FCMEN=OFF, IESO=OFF, PWRT = ON, BOREN=OFF,   WDT=OFF,  WDTPS=128, MCLRE=ON,  LPT1OSC = OFF,PBADEN=OFF,LVP=OFF, DEBUG=OFF// , CPB=OFF,CP0=OFF,CP1=OFF,CP2=OFF, CPD = OFF
#define Today "Build Date = 2/14/2022, (C) Peter C Cranwell, 2016"
#define MaxPacketLength            213    // Maximum Packet Length
ram long                  MatchFireTime = 200;

ram int                   MS;
ram unsigned long         ShowTimeMS; 
ram unsigned long         Timer1MS;
ram char                  WorkBuffer[MaxPacketLength];
#include "string.h"

#include "cranwellP18F2525.h"     /* NOTE: Includes fixes for string and mem functions and all uart functions */
#include "spi.h"
#include "timers.h"
#include "EEP.h"
#include "adc.h" 
#include "PortB.h"
extern unsigned int volatile pMsg ;  
extern unsigned int volatile QHead;
extern unsigned int volatile QTail;    
extern int volatile NQueue;                         





/*****************************************************************************
 '
 '     Downstream RS485 SLAVE    Downstream RS485 SLAVE     Downstream RS485 SLAVE     
 '
 '
 ' Author  P. Cranwell             Date  7/21/2009      Comment
 '                                       8/7/2010       Rewoked for Pic18F2515 
 '                                       8/10/2010      Global Reset 
 '                                       10/13/2010     Reworked for variable message length 
 '                                                        and Live Music Synchronization Mode
 '                                       10/19/2010     Changed message format to accomodate various
 '                                                      work unit formats.  Variable Msg Length
 '                                                      EEPROM Logic
 '                                       3/11/2011      Changes for FB1 Firing Board (PCB).  New Pn assignments
 '                                       5/4/2011       Added back Status info on ACK.
 '                                       5/25/2011      Added Software Version LED blink
 '                                       6/11/2011      Corrected ACK/NAK format.
 '                                       6/22/2011      Changed Triggered Event logic to return to Poll if Delay GT 50MS
 '                                       7/26/2011      Removed Auto Show Logic
 '                                       8/13/2011      Added USART Interrupt Logic
 '                                       8/16/2011      Added Global Load Show, Global Arm, and Expanded Status Information  
 '                                       7/10/2013      Added Message Queueing and redesigned USART interrupt logic.
 '                                       9/2/2013       Added Large Network Support
 '                                       12/16/2013     On temporary power loss, restart session.. Do Not Re-arm.
 '                                       12/22/2013     Added Logical Unit Address logic
 '                                       12/22/2013     Added Logical Unit Fire Port Logic
 '                                        1/03/2014     Fixed Global Arm Logic
 '                                        7/6//2014     Fixed Load Default Show Logic.
 '                                        7/17/2014     Turned OFF BORESET to eliminate Rearm dropout on unit 301
 '                                       11/19/2014     Turned on MCLRE to work with Microchip MCP100 Reset IC
 '                                        9/12/2016     Reduced time in Fire Cuee loop to permit 1MS event spacing
 '                                 
 '~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 '  Slave Pyrotechnic Firing Controller with RS-485 Link to Headend Controller.
 '     This version is for Low Voltage  12-24 Power source.
 ' 
 '/////////////////////////////////Check message formats and correct comments ////////////////////////////////////
 '  Message Structure
 '
 '   All messages have one of the following message formats:
 '
 '   BYTE  1      Message length (Binary) 1 - 255 
 '   BYTE  2      MN = Message Number Modulo 10
 '   BYTES 3-4    Number of Work Units
 '   BYTES 5-7    Unit Address 
 '   BYTES 8-9    Command
 '
 '                      01 = Start
 '                      02 = Stop
 '                      04 = Fire
 '                      05 = Sense
 '                      06 = Trace ON
 '                      07 = ACK
 '                      08 = NAK
 '                      10 = Arm
 '                      11 = Disarm
 '                      12 = Measure HV
 '                      13 = Set Default Show
 '                      14 = Save Show in EEPROM
 '                      19 = Trace OFF
 '                      16 = Start of Programming Triggered Show
 '                      17 = End of Programming
 '                      18 = Assign Area, Caliber, Item to a port. 
 '                      20 = Pause
 '                      21 = Continue
 '                      22 = Enable Port
 '                      23 = Disable Port
 '                      24 = Assign Virtual Address
 '                      25 = Query Build Date 
 '                      08 = Load Show from EEPROM        (Broadcast)
 '                      20 = Pause                        (Broadcast)
 '                      21 = Continue                     (Broadcast)
 '                      33 = Set Unit Address into EEPROM (Broadcast)
 '                      44 = Trigger Cue                  (Broadcast)  
 '                      93 = Show Poll Delay              (Broadcast) 
 '                      94 = Slow Poll					  (Broadcast) 	
 '                      95 = Fast Poll 					  (Broadcast)
 '                      96 = General Poll                 (Broadcast) 
 '                      97 = ShowID                       (Broadcast) 
 '                      98 = Current Time                 (Broadcast)
 '                      99 = Reset                        (Broadcast)  
 ' 
 '
 '      Each Work Unit ( 1 - 16) has the following Format for Command Code "04"   (Show Fire event)
 '
 '
 '        BYTES 1-3     Port Number
 '                      000 = Base Unit
 '                      001-nn are Port numbers     
 '
 '        BYTES 4 - 11  Time Code in MMMMMMMM Milliseconds  (Max 27 hours)
 '
 '
 '
 '
 '
 '      Each Work Unit ( 1 - 16) has the following Format for Command Code "18"   (Assign Area, Caliber, Item to a port) 
 '
 '
 '        BYTES 1-3     Port Number
 '                      000 = Base Unit
 '                      001-nn are Port numbers     
 '
 '        BYTES 4-6     Area
 '        BYTES 7-9     Caliber
 '        BYTES 10-12   Item
 '
 '                 
 '        Last Byte     Check Digit
 '
 '
 '    ///////////////////////   Check message format //////////////////////////
 '
 '   BYTE  1      Message length (Binary) 1 - 255 
 '   BYTE  2      MN = Message Number Modulo 10
 '   BYTES 3-4    Number of Triggered Work Units 
 '   BYTES 5-7    Unit Address
 '
 '   1 - 16 work units (TWU) in the following format
 '
 '
 '        BYTES 1-3     CC  = Cue Number
 '
 '        BYTES 4-6     SS  = Sequence ID
 '
 '        BYTES 7-9     NN  = Next Sequence ID (If Chained)
 '
 '        BYTES 10-17    Time Code in MMMMMMMM Milliseconds  (Max 27 hours)
 '                 
 '        Last Byte     Check Digit
 '
 '
 '
 '====================================================================================
 ' ACK Format
 '
 '   BYTE  1     MN = Same Message Number as received message
 '   BYTE  2-3   WU = Number of Work Units = 1
 '   BYTES 4-6   Host Unit Number = "01"
 '   BYTES 7-8   ACK Command Code
 '   BYTE  8     Message Check Code
 '====================================================================================
 ' NAK Format
 '
 '   BYTE  1      MN = Same Message Number as received message
 '   BYTE  2-3    WU = Number of Work Units 
 '   BYTES 4-6    Host U nit Number = "01"
 '   BYTES 7-8    NAK Command Code
 '   BYTE  6      Message Check Code
 '====================================================================================
 ' PC Master, Relay controller and Slaves  maintain message counters.
 '
 '  Each receiver maintains a next received message number.
 '  Each Transmitter maints a next send message number.
 '
 '  Upon message receipt, the receiver computes the check digit and compares it to
 '  the received message checkdifit.  If they are unequal, it sends a NAK
 '  reply containing the next expected received message number'
 '
 '  If the check digit is correct, the receiver compares the message number
 '    with the next expected received message number.
 '    If not equal, it replies with a NAK specifying the expected
 '    message number.
 '
 '  If both of the above checks are good, the received decodes the message and
 '    executes the command specified in the message.  It then generates an ACK
 '    message and sends it to the Master. It then increments the next expected
 '    received message number.
 '
 '  If the receiver receives a nmessage with an earlier message number, it means that the
 '    master did not receive the ACK and is re-transmitting.  Since the Receiver has already carried out the
 '    last command, it replies with an ACK, using the received message number.  It does not repeat the command
 '    and does not increment the next received message number, as it is already correct.
 '
 '
 '  In this implementation, all messages sent are immediately acknowledged so
 '    the NAK message number should always be the same as the one sent.
 '
 '
 '
 '
 '			PIC18F2525 Pin Usage
 '
 '          Pin 1     Master Reset tied to +5 via 10K resistor.
 '          Pin 2     A/N Input for 12 Volt measurement.
 '          Pin 3     Arming Pulse
 '          Pin 6     Arming Circuit
 '          Pin 5     RS485 T/R Control.  Receive/Transmit LED
 '          Pin 4     Trace Serial Transmit   38.4 KBPS
 '          Pin 8     VSS (Ground)
 '          Pin 19    VSS (Ground)
 '          Pin 20    VDD +5 Volts   
 '          Pin 17    RS485 Transmit          38.4 KBPS
 '          Pin 18    RS485 Receive           38.46 KBPS
 '          Pin 7     Cue 1
 '          Pin 9     Cue 2
 '          Pin 10    Cue 3
 '          Pin 11    Cue 4
 '          Pin 12    Cue 5     
 '          Pin 13    Cue 6     
 '          Pin 14    Cue 7
 '          Pin 15    Cue 8
 '          Pin 16    Cue 9
 '          Pin 21    Cue 10
 '          Pin 22    Cue 11
 '          Pin 23    Cue 12
 '          Pin 24    Cue 13
 '          Pin 25    Cue 14
 '          Pin 26    Cue 15
 '          Pin 27    Cue 16  
 '          Pin 28    Active 
 *****************************************************************************/





///////////////////////// Module Configuration  /////////////////////////
#define MYADDR                  "302"
#define NumPorts                16          
RAM char                        cNumPorts[4] = "016";   
/////////////////////////////////////////////////////////////////////////


#define SoftwareVersion         1
#define TRACE 					1
#define iStartup                1
#define iShutdown               2
#define iUnitReset              99
#define iFire                   4 
#define iSense                  5
#define iTraceON                6
#define iTraceOFF               19     
#define iSaveShow               7
#define iLoadShow               8 
#define iArm                    10
#define iDisarm                 11
#define iSetDefaultShow         13
#define iStartProgramming       16
#define iEndOfProgram           17
#define iPause                  20
#define iContinue               21 
#define iEnablePort             22
#define iDisablePort            23
#define iAssignLogical          24
#define iQueryDate              25
#define None                    0  
#define ACKNAKLength            5   
#define ReplyLength             47


#define DefaultTOT              290         // Receive TOT MS
#define TEventTOT               50          // TEvent Message Polling Time
#define DeadManCount            180         // 15 Sec at Default TOT = 300 MS   (One count every 10 TOT)
#define Out                     1  
#define In                      2
#define TO                      3
#define OK                      4
#define LOW                     0
#define HIGH                    1

// EEPROM Memory Layout
#define EEUnitAddress			0
#define EELogicalAddress        3
#define EEDefaultShow           6
#define EESessionStatus         7
#define EEArmedStatus           8
#define EETEventStatus          10
#define EETEventCheckpoint      0x10

#define EEShowStart             0x100
#define SizeOfUnitAddress       3




void SendACK(void);
void SendNAK(void);
void Receive(char volatile Reply[]);
char IntToCh(int value);
void BumpReceiveMessageNumber(void);
void ResetSlave(void);
void Blip(void);
void Trace(char Msg[], int Direction);
void ResendACKNAK(void);
int  CheckDigit(char Msg[]);
void ParseWorkUnit(void);
void ProcessWorkUnit(void);
void SetTime(void);
void SetStatus(void);
void SetShortStatus(void);
void StartTimer0(void);
void StartTimer1(void);
void OpenUSARTpic18f2515 (unsigned int);
void ShowUnitAddress(void);
void SetUnitAddr(char Data[3]);
void SetLogicalAddr(char Data[3]);
int  GetUnitAddr(void);
int  GetLogicalAddr(void);
void LoadShow(void);
int  GetDefaultShow(void);
void Dump (char *Start, int Len);
int  GetNextTEventDelay(void);
void AllPinsOff(void);
int  GetPortAddress(int Port);
void Fire(int Port);
int PinTest(int Port);
void SetSessionStatus(unsigned char Status);
void SetTEventStatus(unsigned char Status);
unsigned char GetSessionStatus(void);
unsigned char GetTEventStatus(void);
void ShowAddressAndSoftwareVersion(void);
int MeasureVoltage(void);
void OpenUART(void);
void SetArmedStatus(unsigned char Status);
unsigned char GetArmedStatus(void);
void Arm(void);
void DisArm(void);
void ReArm(void);
void CheckpointShowStatus(void);
void RestoreShowStatus(void);
void ResetAllPorts(void);



struct SMPTE
{
    char MS[8]; // Time in Milliseconds up to max of 27.7 Hours
};

struct MsgHeader
{
    char MsgNumber;
    char NumWorkUnits[2];
    char Unit[3];
    char Command[2];
    char Cue[3];
};

struct WorkUnit
{
    char  Port[3];
    struct SMPTE TimeCode;
};
struct TWorkUnit
{
    // Work Units are ordered, withing the message, in Seq order
    char   Cue[3];
    struct SMPTE TimeCode;
    char   Port[3];
    char   NextSeq[3];
};

struct PortControlWorkUnit
{
    char   Port[3];
    char   Area[3];
    char   Caliber[3];
    char   Item[3];
};

struct PortControl
{
    unsigned int Area;
    unsigned int Caliber;
    unsigned int Item;
};

struct TEvent
{
    unsigned long EventTimeMS;   // Cue Time after Fire Event message
    char  Port[4];               // Port Number 
    char  Cue[4];                // Cue Number
    char  Seq[4];                // Sequence Nubmber
    char  NextSeq[4];            // Next in Sequence Number

};

struct PortControl
{
    unsigned int Area;
    unsigned int Caliber;
    unsigned int Item;
};






// =============================    Data Definitions  Access RAM  ========================================

extern unsigned char volatile Buffer[426];
extern char volatile MsgReady;


#pragma udata BIGDATA
#pragma idata IDATA



ram char TodaysDate[] = Today;
ram int intCode;
ram int intPort;
ram int iCheck;
ram char *cp;

union W
{
    char WorkString[19];
    struct MsgHeader Header;
    struct WorkUnit WU;
    struct TWorkUnit TWU;
    struct PortControlWorkUnit;
} WorkArea;



union T
{
    char TimeString[9];
    struct SMPTE Time;
} Clock;




/************************************************************************************************************/
// NOTE:  For the following markers to be valid, you must not use initialized data within
//    the markers.  Initialized data is allocated in a different address pool
//    from the uninitialized data

RAM char                  ShowBegin;                // Address Marker
RAM char                  ShowType[2];
RAM int                   iShowType;
RAM union                 SE
{
    struct TEvent TE;
}  ShowEvents[17];     

RAM struct PortControl    PortActive[17];                        
RAM int                   nQueue;
RAM int                   EndOfWorkQueue;
RAM int                   HeadOfWorkQueue;
RAM char                  ShowEnd;                  // Address Marker
/*************************************************************************************************************/


RAM char                 *ptr;
RAM char                  RS485InString[]       = "==>";
RAM char                  RS485OutString[]      = "<==";
RAM char                  TOString[]            = "TOT";
RAM char                  OKString[]            = "OK ";
RAM char                  ShowStarted[]         = "Show Started"; 
RAM long                  NextEventDelay;
RAM long                  LongWork;

RAM char                  WrkArea[20];
RAM char                  ACKdata[ReplyLength+1];
RAM char                  NAKdata[ReplyLength+1];
RAM char                  RadioReply[ReplyLength+1];
RAM char                  Status[39];
RAM char volatile         Message[MaxPacketLength+1];
RAM struct                WorkUnit ThisMessage;
RAM char                  TraceMsg[MaxPacketLength+1];
RAM int volatile          RecResult;
RAM char                  fStarted = FALSE;
RAM int                   Latency;
RAM int                   LatencyCt;
RAM int                   LatencyTot;
RAM char                  fArm = FALSE;
RAM char                  ACKSENT;
RAM char                  NAKSENT;
RAM float                 fHighVoltage;
RAM float                 fLowVoltage;
RAM char                  cHighVoltage[15] = "00.0";
RAM char                  cLowVoltage[15] = "00.0";
RAM int                   iNextReceiveMessage;
RAM char                  cReceiveMessageNumber[2];
RAM int                   i,j, k;
RAM int                   iCue; 
RAM char                  Cue[3];
RAM char                  Armed;
RAM char                  GlobalReset[]         = "99999999";
RAM char                  Poll[]                = "99996999";
RAM char                  FastPoll[]            = "99995999";
RAM char                  SlowPoll[]            = "99994999";
RAM char                  SetSlowPollDelay[]    = "99993999"; 
RAM char                  PollReply[]           = "10100107";
RAM char                  GlobalArm[]           = "99910999";
RAM char                  GlobalDisarm[]        = "99911999";
RAM char                  GlobalPause[]         = "99920999";
RAM char                  GlobalContinue[]      = "99921999";
RAM char                  GlobalLoadShow[]      = "99908"; 
RAM char                  Reset[]               = "99999";
RAM char                  TimePacket[]          = "99998";
RAM char                  ShowID[]              = "99997999"; 
RAM char                  SetAddr[]             = "99933";  
RAM char                  FireCue[]             = "99944";
RAM char                  LogicalFirePort[]     = "99904";
RAM char                  PortEnable[]          = "99922999";
RAM char                  PortDisable[]         = "99923999"; 
RAM char                  ACKmsg[3]             = "07";
RAM char                  NAKmsg[3]             = "08";
RAM char                  HostAddr[4]           = "001";  
RAM char                  ThisUnit[4]           = MYADDR;
RAM char                  ThisLogicalAddr[4]    = MYADDR;   
RAM char                  LastUnit[4]           = "";
RAM char                  TempUnit[4]           = "\0"; 
RAM char                  CRLF[3]               ="\r\n";
RAM char                  Starting[]            = "Starting V1  11/14/14";
RAM char                  Radioinstring[]       = "<==";
RAM char                  Radiooutstring[]      = "==>";
RAM BIT                   TRC = FALSE;
RAM char                  OneWorkUnit[]         = "01";
RAM char                  Resend = FALSE;
RAM int                   wu;
RAM char                  L[]                   = "LV";
RAM char                  H[]                   = "HV";
RAM char                  RFCommand;
RAM WORD                  Wparm;
RAM char                  Rc;
RAM char                  TimeoutMsg[]          = "Timeout ";
RAM char                  ResetMsg[]            = "Reset";
RAM char                  Reset2Msg[]           = "ResetComplete"; 
RAM char                  IgnoredMsg[]          = "Ignored";
RAM char                  StartShowMsg[]        = "Start Show";
RAM char                  FireMsg[]             = "Fire"; 
RAM int                   DebugMsgNumber = 1; 
RAM int                   TimerZero = 0;
RAM int                   Pin;
RAM int                   ChainCount; 
RAM char                  WorkArea2[40];
RAM char                  Wrk1[122];
RAM char                  Wrk2[40];
RAM int                   Result;
RAM int                   ShowLen;
RAM unsigned              EEAddress;
RAM char volatile         *p;
RAM char volatile         *p2; 
RAM char                  C; 
RAM int                   DefaultShow;
RAM char                  WorkMsg[MaxPacketLength+1];
RAM int                   Len;
RAM char                  StartProgMsg[] = "Start Programming";
RAM char                  EndProgMsg[] = "End of Programming";
RAM char                  TypeT[] = "T";
RAM int                   TESeqIndex;
RAM int                   TEventAgain;
RAM long                  TEventDelay;  
RAM long                  CueOffTime[17];
RAM char                  Seq[4];
RAM char                  NextSeq[4];
RAM int                   SizeOfWU;
RAM int                   TEventPtr;
RAM int                   TEventSeq;
RAM int                   TEventNext; 

/////////////////////////////////////////////////////////////////////////
//     TEvent Variables for Checkpoint, Restore .  Keep to a minimum to minimize EEPROM time.
// Dont initialize these variables.  They should not be initialized at Processor Reset
RAM char                  TEventParameterBegin;
RAM long                  TEventTimeMS; 
RAM int                   CuePtr;  
RAM int                   TEventActive;
RAM int                   ClockStarted;
RAM char                  TEventParameterEnd;
//
//////////////////////////////////////////////////////////////////////////
RAM int                   IgnoreIgnore;
RAM char                  SeqTable[17][4] ={ "  ", "001", "002", "003", "004", "005", "006", "007", "008", "009", "010", "011", "012", "013", "014", "015","016"};

RAM int                   NoMsgNumberIncrement = FALSE;
RAM int                   BlipCtr;
RAM int                   LastFire;
RAM char volatile         *cdata;
RAM char                  ORError[] = "Overrun Error";
RAM char                  FRError[] = "Framing Error";
RAM int                   Voltage;
RAM char                  MessageType[9];
RAM char                  ShowLoaded = '0';  
RAM int                   ProgrammingActive = 0; 

RAM int                   DeadManCounter  = 0;
RAM char                  NumberWorkUnits[] = "00";
RAM char                  cdigit[2]  = "\0";
RAM char                  PortStatus = LOW;
RAM char                  Code[3] = "";
RAM char                  Unit[4] = "";
RAM char                  Port[4] = "";  
RAM char                  Now[9]  = "00000000";
RAM int                   ReturnStatus = 0; 
RAM int                   AnyPinOn = 0;
RAM int                   PollDelay;
RAM long                  SlowPollDelay;
RAM char                  LogicalUnit[4];
RAM char                  NetworkTopology[28];
RAM int                   ResponseDelay;
RAM int                   iThisUnit; 
RAM int                   iBankIndex; 
RAM int                   iTotalDelay;
RAM int                   ArmPending = 0;



union
{
    char WorkString[15];
    struct WorkUnit WorkMessage;
}  WrkUnit;


// Translate port # to Hardware address
RAM char Cues[17] = {0,7,9,10,11,12,13,14,15,16,21,22,23,24,25,26,27};


// NOTE: Boradcast messages do not increment record count and may contain any message number

RAM char TestMsg[]       = "1234567890ABCDEFGHIJ";
RAM unsigned char length;
RAM char Zero[]          = "00000000000000000000";
RAM char BadMsg[]        = "Unknown Message Code";
RAM char SaveShowMsg[]   = "Saving Show To EEPROM";
RAM int  intCue;
RAM int  Ccode; 
RAM int  Ccode2;
RAM int  MsgLength;
RAM long Longi;
RAM long Longj;
RAM long ClockTime;
RAM int  Running = FALSE;
RAM int  fPaused;
RAM int  iPort, iArea, iCaliber, iItem, l;
extern int   volatile iStarting;    




void main(void)
{

    /*
       Disable Watchdog timer
       WDT is used to reset processor if the COMM interrupt routine gets hung.  Under some conditions,
         the comm routine does not complete a message and the controller becomes unresponsive.
       The timer is set when data is received and cleared when the mainline program completes a successful receive operation.
    */


    WDTCON = 0;


    //Set All required ports to I/O
    // Set A/N Chan 0
    ADCON0 = 0x01;           // Channel 0 AN0
    ADCON1 = 0x0E;           // All RA0 = Analog, RA1:7 Digital
    TRISA  = 0x01;           // RA0 = Input
    ADCON2 = 0xDF;           // Right Justified, 20TAD, A/D Clock


    Pin5LOW();

    TRC = FALSE;
    //TRC = TRUE;   
    SetClockSpeed(8);    // Set Internal 8 Mhz Clock
    CloseSPI();
    ClosePORTB();

    OpenUART(); 
    OpenUSARTpic18f2525(38400);      // Hardware UART for RS-485


    nQueue = 0;
    QTail = 1;
    QHead = 0;    // Qhead is incremented before first read is returned.
    NQueue = 0;

    QHead = 0;
    QTail = 0;
    memset((char *)Buffer,'\0',sizeof(Buffer));


   
    ResetSlave();
    GetUnitAddr();
    GetLogicalAddr();
    Running = TRUE;
    iStarting = TRUE;


    //ShowAddressAndSoftwareVersion();



    Trace(Starting,None);







    while (TRUE)
    {

        Blip();    // Generate Arming Pulse on Pin 3. 8MHZ clock & loop results in
                   // 2MS square wave on Pin3


        ShowTimeMS = GetTimeMS();


        if (DeadManCounter > DeadManCount)
        {

            Armed = FALSE;
            Pin6LOW();        // Turn Off Arming Circuit
            fArm = FALSE;
            SetArmedStatus('D');
            CloseTimer0();
            CloseTimer1(); 
            memset((char *)Message,'\0',sizeof(Message));
            ClockStarted = FALSE;
            ResetSlave();
            continue;     // continue main loop
        }



        Receive(Message);

        if (RecResult > 0)
        {


            iStarting = FALSE;

            // Disable Watchdog timer when message is received 
            WDTCON = 0;
            ClrWdt();


            // Copy message type from header to allow "strstr" on just header data
            memset(MessageType,'\0',sizeof(MessageType));
            if (ProgrammingActive == FALSE)
            {
                // Message Type contains 8 bytes of Message Header containing Module Address, Op Code and Port Address.
                memcpy((void *)MessageType, (void *) Message+3,8);
            }
            memset(LogicalUnit,'\0',sizeof(LogicalUnit));
            memcpy((void *)LogicalUnit, (void *) Message+12,3);



            // Get message Check Code
            Ccode2 = atoi(Message+RecResult-1);
            // Zero it out
            memset((void *)Message + RecResult -1,'\0',1);
            // Calculate check code
            Ccode = CheckDigit((char *)Message);

            if ( Ccode != Ccode2)
            {

                memset((char *)Message,'\0',sizeof(Message));
                memset(MessageType,'\0',sizeof(MessageType));

                // If check code is incorrect, do not process it
                continue;     // continue main loop
            }

        }

        // If Result is Neg, no message was received
        // If result > 0, Result = length of received message
        if ((RecResult > 0)|| (TEventActive == 1))
        {

            /********************************** Process the Broadcast Messages in this section **********************************/

            // Check for Time message only if Clock Started

            if ((ClockStarted == TRUE) && (RecResult > 0))
            {
                cp = strstr(MessageType,TimePacket);
                if (cp != NULL)
                {
                    // Process Time packet

                    if (ArmPending == TRUE)
                        ReArm();

                    memset((char *)Message+19,'\0',1);
                    ClockTime = atol(Message +11);
                    ShowTimeMS = ClockTime;

                    StartTimer0();
                    TimerPopCount = ShowTimeMS / 32;
                    memset((char *)Message,'\0',sizeof(Message));

                    // Adjust TeventTime if next event within 20MS of Time Signal
                    //    to prevent bypassing pending events due to program latency.

                    if (TEventTimeMS < ShowTimeMS + 20)
                        TEventTimeMS += 20;

                    continue;
                }



///////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////// Disable Port Message///////////////////////////////////////
//////////////////////This is a broadcast message to All Units/////////////////////////////

//// Message format..............
//
//              N		Message Number
//		+1		WU		Number of Work Units
//      +3      999		Unit Address
//      +6      000     Port Address (Not meaningful)  
//      +9		AAA		Area
//      +12		CCC		Caliber
//      +15     III     Item
//
//      The above fields, if present, are an AND condition
//
//      For example, if AAA and CCC are specified, all field units that have ports
//      connected to CCC Caliber in Area AAA disable the associated ports.
///////////////////////////////////////////////////////////////////////////////////////////


                cp = strstr(MessageType,PortDisable);
                if (cp != NULL)
                {
                    // Process Disable Port, Parse the message here
                    // Message format is:
                    // 	
                    // Msg#, #WU, Unit (999), Command(23), Port (999), Area(3),Caliber(3),Item(3)

                    memset((char*) Message + 18,'\0',1);
                    iItem = atoi(Message + 15);
                    memset((char *) Message + 15,'\0',1);
                    iCaliber = atoi(Message + 12);
                    memset((char *) Message + 12,'\0',1);
                    iArea = atoi(Message + 9);

                    // Process message here
                    // Find all ports that have matching Item, Caliber and Area set.


                    continue;
                }
            }

            // Process Fire Cue Messages

            if (RecResult > 0 && ProgrammingActive == 0)
            {

                cp = strstr(MessageType,FireCue);

                if (cp != NULL)
                {

                    if (ArmPending == TRUE)
                        ReArm();
                    // Fire Cue Message
                    // Extract Cue number from message

                    memset(WorkArea.WorkString,'\0',sizeof(WorkArea));
                    strncpy(WorkArea.WorkString,Message,sizeof(WorkArea.WorkString));
                    memset(WorkArea2,'\0',sizeof(WorkArea2));
                    memcpy(WorkArea2,(void *)WorkArea.Header.Cue,3);
                    TEventPtr = atoi(WorkArea2);
                    CuePtr = 0;

                    // Find Cue in TEvents
                    for (i=1; i < NumPorts+1; i++)
                    {
                        if (atoi(ShowEvents[i].TE.Cue) == TEventPtr)
                        {
                            CuePtr = i;     // CuePtr has Seq # of Show entry.
                            break;
                        }

                    }

                    if (CuePtr == 0)
                    {
                        memset((char *)Message,'\0',sizeof(Message));
                        continue;
                    }
                    // Set Cue fire time and get next event 

                    TEventSeq = atoi(ShowEvents[CuePtr].TE.Seq);
                    TEventNext = atoi(ShowEvents[CuePtr].TE.NextSeq); 
                    TEventTimeMS = ShowEvents[CuePtr].TE.EventTimeMS;
                    // Reset clock time to 0
                    TimerZero = 1;
                    StartTimer0();
                    ClockStarted = TRUE;
                    ShowTimeMS = 0;
                    TEventActive = 1;
                    SetTEventStatus('A');
                    memset(ShowType,'T',1);
                    RecResult = 0;

                }

            }







            // Further processing of Fire Cue Message or TEvent processing

            if (TEventActive == 1)
            {
                while (CuePtr > 0)
                {

                    // Get current time

                    // Skip all CUEs with times < current time
                    // This allows restart a any show time and prevents earlier CUEs from mass firing
                    // Since the loop times out in 75 MS, clock would have to  be fast by 75 MS to miss an event

                    ShowTimeMS = GetTimeMS(); 
                    TEventDelay = TEventTimeMS - ShowTimeMS;

                    // Adjust for time in loop to prevent bypass for time 0 events
                    // This code prevents Event Times < 20 from firing
                    // This may be incorrect


/*       2/19/2022   PCC
                    if (TEventTimeMS < 20)
                    {
                        TEventDelay = 0;
                        ShowTimeMS = 0;
                    }


*/

/* *********************
                    // Bypass all events less than current time
                    // This prevents mass firing events during recovery operation
                    while (TEventDelay < 0)
                    {
                        TEventDelay = TEventTimeMS - ShowTimeMS;

                        if (TEventDelay < 0)
                        {
							// Step thru until Event Time > Show Time or until End of Event Chain
                            if (atoi(ShowEvents[CuePtr].TE.NextSeq) > 0 )
                            {
                                CuePtr = atoi(ShowEvents[CuePtr].TE.NextSeq);
                                TEventTimeMS = ShowEvents[CuePtr].TE.EventTimeMS;
                            } else
                            {
                                TEventActive = 0;
                                SetTEventStatus(' ');
                                CuePtr = 0;
                                IgnoreIgnore=1;
                                memset((char *)Message,'\0',sizeof(Message));
                                continue;
                            } 
                        }
                    } 
*/  ///////////////////////////////////////////////////////





                    // If Time to event < TOT ( currently 50 MS) then process it now,
                    //       else continue looping on Receive
                    if (TEventDelay < TEventTOT)
                    {
                        DelayMS((int)TEventDelay);
                        iCue = atoi(ShowEvents[CuePtr].TE.Port);
                        if (ArmPending == TRUE)
                            ReArm();
                        Fire(iCue);

                        if (atoi(ShowEvents[CuePtr].TE.NextSeq) > 0 )
                        {
                            CuePtr = atoi(ShowEvents[CuePtr].TE.NextSeq);
                            TEventTimeMS = ShowEvents[CuePtr].TE.EventTimeMS;
                            // Dont Checkpoint on events that are too close in time
                          //  if (TEventTimeMS > ShowTimeMS + 100)
                          //      CheckpointShowStatus();
                        } else
                        {
							// End of Event Chain
                            TEventActive = 0;
                            SetTEventStatus(' ');
                            CuePtr = 0;
                            IgnoreIgnore=1;
                            memset((char *)Message,'\0',sizeof(Message));
                            continue;     // continue main loop

                        }

                    } else
                    {
                        break;
                    }
                }   // End while

            }  // Not a Triggered Show   (This is an obsolete test.  TODO: Remove show type since all shows are Triggered)

        } 






        if (RecResult > 0)
        {


            //-----------------------------------------------------------
            //   Process Show Unit ID broadcast message
            //-----------------------------------------------------------
            cp = strstr(MessageType,ShowID);
            if (cp != NULL)
            {
                ShowAddressAndSoftwareVersion();
            }


            //------------------------------------------------------------
            // Set Unit Address
            //------------------------------------------------------------

            // Two consecutive Set Address messages required to set unit address
            cp = strstr(MessageType,SetAddr);
            if (cp != NULL)
            {

                strncpy(ThisUnit,Message+8,3);
                if (strcmp(LastUnit,ThisUnit) == 0)
                {

                    SetUnitAddr(ThisUnit);
                    GetUnitAddr();
                    ShowUnitAddress();
                    memset((char *)Message,'\0',sizeof(Message));
                } else
                {
                    strcpy(LastUnit,ThisUnit);
                } 

                continue;
            } else
            {
                memset(LastUnit,'\0',sizeof(LastUnit));
            }
        } else
        {

            continue;
        }




        //------------------------------------------------------------------- 
        // Logical Unit Fire Port Broadcast message
        //-------------------------------------------------------------------

        cp = strstr(MessageType,LogicalFirePort);
        if (cp!= NULL)
        {

            // If Logic Fire is for this Logical Unit
            if (strcmp(LogicalUnit,ThisLogicalAddr) == 0)
            {
                if (ArmPending == TRUE)
                    ReArm();

                if (Armed == TRUE)
                {
                    memset(Port,'\0',sizeof(Port));
                    memcpy(Port,(void *)Message + 8,3);  
                    intPort = atoi(Port); 
                    Fire(intPort);
                }
            }
            continue;
        }


        //------------------------------------------------------------
        // Check for Global Arm (Broadcast)
        //------------------------------------------------------------

        cp = strstr(MessageType,GlobalArm);
        if (cp!= NULL)
        {
            if (fStarted == TRUE)
            {
                Arm();
            }
            continue;
        }




        //--------------------------------------------------------------
        //  Process Global Disarm Broadcast message
        //--------------------------------------------------------------

        cp = strstr(MessageType,GlobalDisarm);
        if (cp!= NULL)
        {
            DisArm();

            continue;
        }






        //-------------------------------------------------------------
        //    Global Load Show Broadcast message
        //-------------------------------------------------------------

        // Change this logic to look in specific record position
        cp = strstr(MessageType,GlobalLoadShow);
        if (cp!= NULL)
        {
            memset(Port,'\0',sizeof(Port));
            memcpy(Port,(void *)Message + 8,3);  
            intPort = atoi(Port); 
            LoadShow();
            memset((char *)Message,'\0',sizeof(Message));
            continue;
        }




        //---------------------------------------------------------------------------
        // Check for Immediate Shutdown (Broadcast Message)
        //---------------------------------------------------------------------------

        // PCC Modify this message logic to work with Program Loading Data

        cp = strstr(MessageType,GlobalReset);
        if (cp!= NULL)
        {
            fStarted = FALSE;
            SetSessionStatus(' ');
            SetTEventStatus(' ');
            ResetSlave();
            continue;
        }


        //---------------------------------------------------------------
        // Check for Poll Broadcast
        // Used when all units are 16 Ports
        //---------------------------------------------------------------

        cp = strstr(MessageType,Poll);
        if (cp!= NULL)
        {
            memset(ACKdata,'\0',sizeof(ACKdata));
            strncpy(ACKdata,PollReply,sizeof(PollReply));
            SetStatus();
            strcat(ACKdata,Status);
            strncat(ACKdata,ThisUnit,3);
            strncat(ACKdata,ThisLogicalAddr,3);
            cdigit[0]=  IntToCh(CheckDigit(ACKdata));
            strncat(ACKdata,cdigit,1); 

            // Set RS485 Transmit mode
            PollDelay = atoi(ThisUnit) * 10;
            DelayMS((unsigned long)PollDelay);   // delay to allow other units to respond without collision
            // Set RS485 Transmit mode
            Pin5HIGH();
            Len = strlen(ACKdata);
            vputsUSART(ACKdata,Len);
            DelayMS(1);   // wait for data to clear RS-485 port before dropping the data direction signal
            continue;
        }



        //----------------------------------------------------------------
        // Check for Fast Poll Broadcast
        //----------------------------------------------------------------

        cp = strstr(MessageType,FastPoll);
        if (cp!= NULL)
        {

            memset(ACKdata,'\0',sizeof(ACKdata));
            memset(NetworkTopology,'\0',sizeof(NetworkTopology));
            memcpy((void *)NetworkTopology,(void *)Message+ 11,sizeof(NetworkTopology));
            strncpy(ACKdata,PollReply,sizeof(PollReply));
            SetShortStatus();
            strcat(ACKdata,Status);
            strncat(ACKdata,ThisUnit,3);
            strncat(ACKdata,ThisLogicalAddr,3);
            cdigit[0]=  IntToCh(CheckDigit(ACKdata));
            strncat(ACKdata,cdigit,1); 

            // Calculate Response Delay

            iThisUnit = atoi(ThisUnit);
            iBankIndex = atoi(ThisUnit)/100;

            ResponseDelay = iThisUnit - (iBankIndex * 100);




            // Get delays for all lower Banks
            for (i=0; i<iBankIndex; i++)
            {
                memset(Wrk1,'\0',sizeof(Wrk1));
                memcpy((void *)Wrk1,(void *)NetworkTopology + (i*3)+1,2);    //Units and Tens position of highest unit number in bank
                ResponseDelay += atoi(Wrk1);

            }



            // Delay 10 MS per responding unit for this unit and all preceeding units
            // 7 MS is min required delay, 10MS is allocated for safety.
            // An additional 20MS is added per bank to adjust for Radio Delays if
            //   separate radios are operational on each address bank.
            PollDelay = (ResponseDelay  * 10) + (iBankIndex * 20);

            DelayMS((unsigned long)PollDelay);   // delay to allow other units to respond without collision


            // Set RS485 Transmit mode
            Pin5HIGH();
            Len = strlen(ACKdata);
            vputsUSART(ACKdata,Len);
            DelayMS(1);   // wait for data to clear RS-485 port before dropping the data direction signal
            Pin5LOW();
            continue;
        }




        //------------------------------------------------------------------
        //   Slow Poll Broadcast message
        //------------------------------------------------------------------

        cp = strstr(MessageType,SlowPoll);
        if (cp!= NULL)
        {
            memset(ACKdata,'\0',sizeof(ACKdata));
            memset(NetworkTopology,'\0',sizeof(NetworkTopology));
            memcpy((void *)NetworkTopology,(void *)Message+ 11,sizeof(NetworkTopology));
            strncpy(ACKdata,PollReply,sizeof(PollReply));
            SetStatus();
            strcat(ACKdata,Status);
            strncat(ACKdata,ThisUnit,3);
            strncat(ACKdata,ThisLogicalAddr,3);
            cdigit[0]=  IntToCh(CheckDigit(ACKdata));
            strncat(ACKdata,cdigit,1); 

            // Calculate Response Delay if delay has not been set

            if (SlowPollDelay == 0)
            {



                iThisUnit = atoi(ThisUnit);
                iBankIndex = atoi(ThisUnit)/100;

                ResponseDelay = iThisUnit - (iBankIndex * 100);




                // Get delays for all lower Banks
                for (i=0; i<iBankIndex; i++)
                {
                    memset(Wrk1,'\0',sizeof(Wrk1));
                    memcpy((void *)Wrk1,(void *)NetworkTopology + (i*3)+1,2);
                    ResponseDelay += atoi(Wrk1);

                }

                // Delay 120 MS per responding unit
                SlowPollDelay = ResponseDelay  * 120;
            }



            DelayMS((unsigned long)SlowPollDelay);   // delay to allow other units to respond without collision


            // Set RS485 Transmit mode
            Pin5HIGH();
            Len = strlen(ACKdata);
            vputsUSART(ACKdata,Len);
            DelayMS(1);   // wait for data to clear RS-485 port before dropping the data direction signal
            continue;
        }



        //-------------------------------------------------------------------
        // Set Slow Poll Delay
        //-------------------------------------------------------------------

        cp = strstr(MessageType,SetSlowPollDelay);
        if (cp!= NULL)
        {
            memset(Wrk2,'\0',sizeof(Wrk2));
            memcpy((void *)Wrk2,(void *)Message+11,2);
            i = atoi(Wrk2);
            // i now has number of Address,delay pair in the message
            memset(Wrk2,'\0',sizeof(Wrk2));

            // Look for This Address in message
            for (j=0; j<i; j++)
            {
                k = j*8;
                memcpy((void *)Wrk2,(void *)Message+13 + k,3);
                if (strcmp(Wrk2,ThisUnit) == 0)
                {
                    // This address found, get the delay.
                    memset(Wrk2,'\0',sizeof(Wrk2));
                    memcpy((void *)Wrk2,(void *)Message+16+k,5);
                    SlowPollDelay = atol(Wrk2);


                }


            }

            continue;
        }


        ////////////////////////////////////////////////////////////////////////////  Unit Specific Message Processing /////////////////////////////////////////////////////

        //-------------------------------------------------------------------
        // Check to see if the message is for this unit
        //-------------------------------------------------------------------
        strncpy(WorkArea.WorkString,Message,sizeof(WorkArea.WorkString));
        if (strncmp(WorkArea.Header.Unit,ThisUnit,3) == 0)
        {
            // The message is for This unit

            // Check Check Code to see if it's a valid message


            // Check to see if it is a Unit Reset
            cp = strstr(MessageType,Reset);
            if (cp!= NULL)
            {
                fStarted = FALSE;
                SetSessionStatus(' ');
                SetTEventStatus(' ');
                ResetSlave();
                continue;
            }


            // Check to see if it is the expected number
            memset(cReceiveMessageNumber,'\0',sizeof(cReceiveMessageNumber));
            memcpy(cReceiveMessageNumber,(void *)&WorkArea.Header.MsgNumber,1);


            if (atoi(cReceiveMessageNumber) == iNextReceiveMessage)
            {

                // Good Message, Prcess it

                memset(Code,'\0',sizeof(Code));
                strncpy(Code,WorkArea.Header.Command,2);
                intCode = atoi(Code);
                // Get the number of Work Units
                strncpy(NumberWorkUnits,WorkArea.Header.NumWorkUnits,2);

                for (wu=0; wu<atoi(NumberWorkUnits); wu++)
                {
                    // Parse the Work Unit
                    memset(WorkArea.WorkString,'\0',sizeof(WorkArea.WorkString));
                    ptr = Message + 8;
                    ptr +=  SizeOfWU * wu; 
                    strncpy(WorkArea.WorkString,ptr,sizeof(WorkArea.WorkString)-1);

                    ParseWorkUnit();

                    if (ProgrammingActive == 0)
                    {
                        // Process Work Unit if not programming
                        ProcessWorkUnit();

                    } else
                    {
                        // Programming Active

                        if (EndOfWorkQueue < NumPorts+1)
                        {
                            // Queue Work Unit  if array has room

                            // Message WUs must be in Seq order

                            i = wu+1 + TESeqIndex;

                            memcpy(ShowEvents[i].TE.Seq,( void *)SeqTable[i],3);
                            memcpy(Seq,(void *)ShowEvents[i].TE.Seq,3);

                            memcpy(ShowEvents[i].TE.NextSeq,(void *)NextSeq,3); 

                            memset(WrkArea,'\0',sizeof(WrkArea));
                            strncpy(WrkArea,Clock.TimeString,8);
                            LongWork = atol(WrkArea);
                            ShowEvents[i].TE.EventTimeMS = LongWork;

                            memset(ShowEvents[i].TE.Port,'\0',sizeof(ShowEvents[0].TE.Port)); 
                            strncpy(ShowEvents[i].TE.Port,Port,3);

                            memset(ShowEvents[i].TE.Cue,'\0',sizeof(ShowEvents[0].TE.Cue)); 
                            strncpy(ShowEvents[i].TE.Cue,Cue,3);

                            memset(Wrk1,'\0',sizeof(Wrk1));

                            if (TRC == TRUE)
                            {
                                sprintf(Wrk1,"Queue T%s C%s P%s S%s N%s", Clock.TimeString, Cue, Port, Seq, NextSeq);
                                Trace(Wrk1,None);
                            }


                            EndOfWorkQueue ++;
                            nQueue = EndOfWorkQueue-HeadOfWorkQueue;
                            if ( wu == atoi(NumberWorkUnits)-1)
                            {
                                // Revert back to normal WU processing
                                ProgrammingActive = 0;
                                TESeqIndex = wu + 1;
                                SizeOfWU = 11;
                                memset(ShowType,'A',1);
                            }

                            continue;

                        }

                        else
                        {
                            SendNAK();
                            memset((char *)Message,'\0',sizeof(Message));

                            continue;
                        }

                    } 


                }  // End For this work Unit
                SendACK();
                memset((char *)Message,'\0',sizeof(Message));

                continue;

            }  // endif Message Processing (Expected Message Number)

            if (atoi(cReceiveMessageNumber) < iNextReceiveMessage)
            // Master did not receive original ACK or NAK, Re- send it.
            {
                ResendACKNAK();
                Resend = FALSE;
            }
            if (atoi(cReceiveMessageNumber) > iNextReceiveMessage)
            // This should never occur. If it does, It's an error in the Relay Controller logic.
            {
                SendNAK();
            }

        }   // End processing for message for This Unit
        else

        {
            // Ignore Messages not for this unit

        }  
        memset((char *)Message,'\0',sizeof(Message));




    }  // end while TRUE (Continuous Loop)


}  // end Main



void SendACK(void)
{
    int ln;

    memset(ACKdata,'\0',sizeof(ACKdata));
    strncpy(ACKdata,Message,1);
    strncat(ACKdata,OneWorkUnit,2);
    strncat(ACKdata,HostAddr,3);
    strncat(ACKdata,ACKmsg,2);

    if ( ReturnStatus == 1)
    {
        SetStatus();
        strncat(ACKdata,Status,22);
        ReturnStatus = 0;

    }

    if ( ReturnStatus == 2)
    {
        SetStatus();
        strcat(ACKdata,TodaysDate);
        ReturnStatus = 0;

    }
    cdigit[0]=  IntToCh(CheckDigit(ACKdata));
    strncat(ACKdata,cdigit,1); 



    DelayMS(10);   // Delay to allow Mesh Radios time to turnaround
    // Set RS485 Transmit mode
    Pin5HIGH();
    Pin3HIGH();
    DelayMS(10);   // delay to allow Mesh Radios time to turnaround 
    ln = strlen(ACKdata);
    vputsUSART(ACKdata,ln);
    DelayMS(1);   // wait for data to clear RS-485 port before dropping the data direction signal
    // Set RS485 Receive Mode
    Pin5LOW();
    Pin3LOW();
    if (Resend == FALSE)
    {
        if (NoMsgNumberIncrement == FALSE)
        {
            BumpReceiveMessageNumber();
        }
    }

    Trace(ACKdata,Out);

    ACKSENT=TRUE;
    NAKSENT=FALSE;
    NoMsgNumberIncrement = FALSE;

}

void SendNAK(void)
{
    int l;

    memset(NAKdata,'\0',sizeof(NAKdata));
    strncpy(NAKdata,Message,1);
    strncat(NAKdata,OneWorkUnit,2);
    strncat(NAKdata,HostAddr,3);
    strncat(NAKdata,NAKmsg,2);
    if (ReturnStatus == 1)
    {
        SetStatus();
        strncat(NAKdata,Status,21);
        ReturnStatus = 0;
    }
    cdigit[0]=  IntToCh(CheckDigit(NAKdata));
    strncat(NAKdata,cdigit,1); 
    l = strlen(NAKdata);
    DelayMS(10);   // Delay to allow Mesh Radios time to turn around    
    // Set RS485 Transmit mode
    Pin5HIGH();
    Pin3HIGH();
    DelayMS(10);   // Delay to allow Mesh Radios time to turn around                            
    vputsUSART(NAKdata,l);
    DelayMS(1);   // wait for data to clear RS-485 port before dropping the data direction signal
    // Set RS485 Receive Mode
    Pin5LOW();
    Pin3LOW();

    Trace(NAKdata,Out);

    NAKSENT=TRUE;
    ACKSENT=FALSE;
}

void ParseWorkUnit(void)
{



    // SMPTE Time Code
    memset(Clock.TimeString,'\0',sizeof(Clock.TimeString));
    strncpy(Clock.TimeString,WorkArea.WU.TimeCode.MS,8);

    //if (strncmp(ShowType,TypeT,1) == 0)
    if (ProgrammingActive == TRUE)
    {
        memset(NextSeq,'\0',sizeof(NextSeq));
        memcpy(NextSeq,(void *)WorkArea.TWU.NextSeq,3);
        memset(Port,'\0',sizeof(Port));
        memcpy(Port,(void *)WorkArea.TWU.Port,3);   
        memset((char *)Cue,'\0',sizeof(Cue));
        memcpy(Cue,(void *)WorkArea.TWU.Cue,3);
        intPort = atoi(Port);
    } else
    {
        memset(Port,'\0',sizeof(Port));
        memcpy(Port,(void *)WorkArea.WU.Port,3);  
        intPort = atoi(Port); 

    }



}

void ProcessWorkUnit(void)
{
    // Process Work Unit

    switch (intCode)
    {
    case iStartup:
        fStarted = TRUE;
        fPaused = FALSE;
        Pin28HIGH();           // Session Up
        SetSessionStatus('S');

        break;
    case iShutdown:
        fStarted = FALSE;
        Pin28LOW();            // Session Down 
        Armed = FALSE;
        SetArmedStatus('D');
        Pin6LOW();             // Disarm
        Pin5LOW();             // RS485 Receive
        SetSessionStatus(' ');
        SetTEventStatus(' ');
        break;
    case iUnitReset:
        NoMsgNumberIncrement = TRUE;         // Prevent Msg Number increment
        SetSessionStatus(' ');
        SetTEventStatus(' ');
        ResetSlave();
        break;
    case iEndOfProgram:
        Trace(EndProgMsg,None); 
        SizeOfWU = 11;
        ProgrammingActive = 0;
        break;
    case iFire:
        if (ArmPending == TRUE)
            ReArm();
        if (Armed == TRUE)
        {
            Fire(intPort);
        }
        break;
    case iSense:
        ReturnStatus = 1;
        // ACK will add status when Return Status = 1 
        //SetStatus();

        break;
    case iAssignLogical:
        strcpy(ThisLogicalAddr,Port);
        SetLogicalAddr(ThisLogicalAddr);
        break;
    case iTraceON:
        TRC=TRUE;
        break;
    case iTraceOFF:
        TRC = FALSE;
        break;
    case  iArm:
        Arm();
        //ArmPending = TRUE;
        break;
    case  iDisarm:
        DisArm();
        break;
    case  iSaveShow:

        // It takes 2 seconds to write program to EEProm
        // Adjust ACK Timeout on Host accordingly.

        ShowLen = &ShowEnd - &ShowBegin;
        EEAddress = ((intPort-1) * ShowLen) + EEShowStart;


        for (i = 0; i<ShowLen; i++)
        {
            p = (char  volatile *)EEAddress + i;
            Busy_eep();
            p2 = (char volatile *)&ShowBegin + 1 + i;
            C = *p2;
            Write_b_eep((unsigned int)p,(unsigned char)C);
            ClrWdt();

        }


        break;


    case  iLoadShow:

        LoadShow();
        //memcpy((void *)&ShowLoaded,&DefaultShow,1);

        //memcpy(&ShowLoaded,(void *)Port+1,1);
        break;
    case  iSetDefaultShow:
        Busy_eep();
        C = (char) intPort + 48;     // Convert to ASCII
        Write_b_eep((unsigned int)EEDefaultShow,(unsigned char)C);
        DelayMS(10);
        memset(Wrk2,'\0',sizeof(Wrk2));
        sprintf(Wrk2,"Setting Default Show = %c",C);
        Trace(Wrk2,None);

        break;


    case  iStartProgramming:
        Trace(StartProgMsg,None);
        ProgrammingActive = 1;
        memset(ShowType,'\0',sizeof(ShowType));
        memset(Cue,'\0',sizeof(Cue));
        strncpy(Cue,(char *)WorkArea.WU.Port,3);
        iCue = atoi(Cue); 

        if (iCue == 2)
        {
            memset(ShowType,'T',1);
            SizeOfWU = 17;
            iShowType = 2;
        }
        break;

    case      iPause:
        fPaused = TRUE;
        break;

    case      iContinue:
        fPaused = FALSE;
        break;
    case      iQueryDate:
        ReturnStatus = 2;
        // ACK will add status when Return Build Date when Status = 2 
        break;

    default:;
        Trace (BadMsg,None);
        SendNAK;
        break; 
    }  // end switch


}


void SetStatus(void)
{

    int Voltage;
    char CVoltage[6];

    memset(Status,'\0',sizeof(Status));

    memcpy((void *)Status,(void *)cNumPorts,3);

    // Set Individual Port Status
    for (intPort=1; intPort<NumPorts+1; intPort++)
    {

        // The PIC requirews two reads to get correct pin state

        PortStatus =  PinTest(GetPortAddress(intPort));
        PortStatus =  PinTest(GetPortAddress(intPort));
        if (PortStatus == LOW)
        {
            Status[intPort+2] = '0';
        } else
        {
            Status[intPort+2] = '1';
        }
    }


    if (Armed == TRUE)
    {
        Status[NumPorts+3] = '1';
    } else
    {
        Status[NumPorts+3] = '0';
    }

    Status[NumPorts+4] =  ShowLoaded;

    Voltage = MeasureVoltage();
    memset(CVoltage,'0',sizeof(CVoltage));
    sprintf(CVoltage,"%04d",Voltage);
    strcat(Status,CVoltage);

}

void SetShortStatus(void)
{

    int Voltage;
    char CVoltage[6];

    memset(Status,'\0',sizeof(Status));

    memcpy((void *)Status,(void *)cNumPorts,3);

    if (Armed == TRUE)
    {
        Status[3] = '1';
    } else
    {
        Status[3] = '0';
    }

    Status[4] =  ShowLoaded;

    Voltage = MeasureVoltage();
    memset(CVoltage,'0',sizeof(CVoltage));
    sprintf(CVoltage,"%04d",Voltage);
    strcat(Status,CVoltage);

}


void ResendACKNAK(void)
{
    Resend = TRUE;

    if (ACKSENT==TRUE)
    {
        SendACK();
    } else
    {
        SendNAK();
    }

}

int CheckDigit(char Msg[])
{

    // calculate check digit according to Bank Routing Number Check Code algorithm.
    // All data must be ASCII 0-9

    int sum, i,d,c,cd;
    int IntTemp;
    char cTemp[2];
    int factor[] = {3,7,1};

    cd = 0;
    memset(cTemp,'\0',2);
    i=0;

    for (c=0; c< strlen(Msg); c++)
    {
        if (i>2 ) i=0;
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




void Receive(char volatile Mesg[])
{
    // Set RS485 Receive Mode
    Pin5LOW();
    RecResult = vgetsUSART(Mesg,MaxPacketLength,1);
    if (RecResult > 0)
    {
        // Clear UART Buffer and msg pointer
        if (TRC == TRUE)
        {
            // memset(Wrk1,'\0',sizeof(Wrk1));
            // sprintf(Wrk1,"==> %d %s \r\n",RecResult,Mesg);
            //Trace(Wrk1,None);
            Trace(Message,In);


            if (UARTErrorOR > 0)
            {
                UARTErrorOR = 0;
                Trace(ORError,None);
            }

            if (UARTErrorFR > 0)
            {
                UARTErrorFR = 0;
                Trace(FRError,None);

            }

        }


        DeadManCounter = 0;

    }
    AllPinsOff();

    Timer1MS += ResetTimer1();
    if (Timer1MS > DefaultTOT)
    {
        Timer1MS = 0;
        if (Armed == TRUE)
        {
            DeadManCounter++;
        }
    }
    return;
}




char IntToCh(int value)
{
    char atab[11] = "0123456789";
    return atab[value];
}



void BumpReceiveMessageNumber(void)
{
    iNextReceiveMessage += 1;
    if (iNextReceiveMessage >9)
    {
        iNextReceiveMessage = 1;  // (it will be incremented to 1)
    }
}


void ResetSlave(void)
{

    unsigned char C;
    char EndMsg[17] = "Reset Complete";

    Trace(ResetMsg,None);
    iNextReceiveMessage = 1;
    memset(cReceiveMessageNumber,'\0',sizeof(cReceiveMessageNumber));
    Pin3LOW();
    Pin5LOW();
    Pin6LOW();
    Pin28LOW();

  
    ResetAllPorts();

    TMR0L = 0;
    TMR0H = 0;
    TimerPopCount = 0;
    ShowTimeMS = 0;

    Armed = FALSE;
    fStarted = FALSE;
    ClockStarted = FALSE;
    RecResult =0;
    HeadOfWorkQueue = 1;
    EndOfWorkQueue = 1;
    nQueue = 0;
    QTail = 0;
    QHead = 0;
    NQueue = 0;
    nQueue = 0;
    memset(Status,'\0',sizeof(Status));
    DeadManCounter = 0;
    Len = (char *) &ShowEnd - (char *) &ShowBegin;
    memset(&ShowBegin,'\0',Len);
    memset(ShowType,'\0',sizeof(ShowType));
    memset(Seq,'\0',sizeof(Seq));
    memset(NextSeq,'\0',sizeof(NextSeq));
    SizeOfWU = 0;
    ProgrammingActive = 0;
    SizeOfWU = 11;
    memset(Status,'\0',sizeof(Status));
    memset(WorkMsg,'\0',sizeof(WorkMsg));
    TESeqIndex = 0;
    TEventAgain = 0;
    TEventActive = 0;
    BlipCtr = 0;
    LastFire = 0;
    ShowLoaded = '0';
    // Clear Interrupt Driven USART Data
    MsgReady = FALSE;
    memset((char *)Buffer,'\0',sizeof(Buffer));
    CloseTimer0();
    CloseTimer1();
    memset((char *)Message,'\0',sizeof(Message));


    // Show Reset on Leds

    Pin28HIGH();
    DelayMS(50);
    Pin28LOW();


    // Get EEPROM Default Show, if set
    DefaultShow = GetDefaultShow();

    if (DefaultShow > 0)
    {
        intPort = DefaultShow - 0x30;
        if (intPort > 0)
        {

            LoadShow();
            memcpy((void *)&ShowLoaded,(void *)&DefaultShow,1);

        }
    }

    C = GetSessionStatus();
    if (C == 'S')
    {
        fStarted = TRUE;
        Pin28HIGH();           // Session Up
    }



    C = GetTEventStatus();
    if (C == 'A')
    {
        //TEventActive = 1;
        RestoreShowStatus();
        StartTimer0();
    }

// Add Get Armed Status logic Here.   If Armed Status = 'A' then set armed pending, waiting for Time Pulse or other command.

    C = GetArmedStatus();
    if (C == 'A')
    {
        ArmPending = TRUE;
        ClockStarted = TRUE;
    } else
    {
        ArmPending = FALSE;

    }


    Trace(EndMsg,None);   

}

void Blip(void)
{

    RAM int   BlipValue;
    static int Pin3;



    if (Armed == TRUE)
    {

        // Toggle Pin 3 to generate Arming pulse
        if (Pin3 == 1)
        {


            Pin3 = 0;
            Pin3LOW();

        } else
        {
            Pin3 = 1;
            Pin3HIGH();
        }

        BlipCtr ++;

        if (BlipCtr > 1200)
        {

            DeadManCounter ++;
            BlipCtr = 0;
        }

    } else
    {
        Pin3LOW();
    }

}


void ShowUnitAddress(void)
{
    int i,j,k,l;

    char MyAddr1[2] = "";
    char MyAddr2[2] = ""; 
    char MyAddr3[2] = "";


    strncpy(MyAddr1,ThisUnit,1);
    strncpy(MyAddr2,ThisUnit+1,1);
    strncpy(MyAddr3,ThisUnit+2,1);

    j = atoi(MyAddr1);
    k = atoi(MyAddr2);
    l = atoi(MyAddr3);



    for (i=0; i< j; i++)
    {

        Pin3HIGH();
        DelayMS(300);
        Pin3LOW();
        DelayMS(300);

    } 


    DelayMS(500);

    for (i=0; i< k; i++)
    {

        Pin3HIGH();
        DelayMS(300);
        Pin3LOW();
        DelayMS(300);

    } 

    if (j>0 && k==0)
    {
        Pin3HIGH();
        DelayMS(600);
        Pin3LOW();
        DelayMS(300);
    }

    DelayMS(500);

    for (i=0; i< l; i++)
    {

        Pin3HIGH();
        DelayMS(300);
        Pin3LOW();
        DelayMS(300);

    } 
    if ((j>0 || k>0)  && l==0)
    {
        Pin3HIGH();
        DelayMS(600);
        Pin3LOW();
        DelayMS(300);
    }


}







void Trace(char *Msg, int Direction)
{


#ifdef TRACE
    int len;

    if (TRC == TRUE)
    {

        memset(TraceMsg,'\0',sizeof(TraceMsg));

        if (Direction == Out)
        {
            strncat(TraceMsg,RS485OutString,3);
        }
        if (Direction == In)
        {
            strncat(TraceMsg,RS485InString,3);
        }

        if (Direction == TO)
        {
            strncat(TraceMsg,TOString,3);
        }
        if (Direction == OK)
        {
            strncat(TraceMsg,OKString,3);
        }

        strcat(TraceMsg,Msg);
        strncat(TraceMsg,CRLF,2);


        //len = strlen(TraceMsg);

        // Write via software UART on Pin 4  at 38400 BPS
        putsUART(TraceMsg);


    }
#endif
}






int GetPortAddress(int Port)
{
    return Cues[Port];
}


void StartTimer0(void)
{

    union Timers timer;
    OpenTimer0(TIMER_INT_ON & T0_16BIT & T0_SOURCE_INT & T0_EDGE_RISE );   
    timer.lt = 1701;     // Set timer to overflow every 32 MS
    // Value determined by calculation and then observation of timer overflow in Simulator
    TMR0H = timer.bt[1]; // Write low byte to Timer0
    TMR0L = timer.bt[0]; // Write high byte to Timer0

    // One Timer pop = 32MS
    TimerPopCount = 0;

}

void StartTimer1(void)
{
    // One count per microsecond at 8 MHZ
    OpenTimer1(TIMER_INT_OFF & T1_SOURCE_INT  & T1_PS_1_8 & T1_OSC1EN_OFF);
    Timer1MS = 0;
    WriteTimer1(0);
}







void LoadShow(void)
{
    int i, Len;

    memset(Wrk2,'\0',sizeof(Wrk2));
    sprintf(Wrk2,"Loading Show from slot %d",intPort );

    if ((intPort >0) && (intPort <3))
    {
        Trace(Wrk2,None);
        Len = &ShowEnd - &ShowBegin;
        memset(&ShowBegin, '\0', Len);
        // intPort contains show slot number
        EEAddress = ((intPort-1) * Len) + EEShowStart;
        memset(Wrk2,'\0',sizeof(Wrk2));
        sprintf(Wrk2,"EEAddress = %d",EEAddress);
        Trace(Wrk2,None);

        for (i=0; i<Len; i++)
        {
            Busy_eep();
            C = Read_b_eep(EEAddress + i);
            p = &ShowBegin + 1 + i;
            *p = C;
        }


        //Dump((char*) &ShowBegin,Len);
        memset(ShowType,'T',1);
        EndOfWorkQueue = 0;
        //sprintf(&ShowLoaded,"%1d",intPort);

    }

}







void Dump (char *Start, int Len)
{
    // Dump memory contents in Hex Formit.  16 Bytes per line.
    int count, remainder,i,j,k;
    char Line[40] = "";
    char HexTable[] = "0123456789ABCDEF";
    char Byte, C1, C2;
    char *p;

    count = Len / 16;
    remainder = Len- (count * 16);

    for (i=0; i<count; i++)
    {
        memset(Line,'\0',sizeof(Line));
        for (j=0; j<16; j++)
        {
            p = Start + (i*16) + j;
            C2 = HexTable[*p & 0x0F];
            C1 = HexTable[(*p & 0xF0) >> 4];

            Line[j*2] = C1;
            Line[(j*2)+1] = C2;
        }
        Trace(Line,None);
    }
}


void AllPinsOff(void)
{
    int i,Pin, APinIsOn;
    char TraceMsg[40];

    if (AnyPinOn == 0) return;    

    AnyPinOn = 0;
    APinIsOn = 0;

    ShowTimeMS = GetTimeMS();


    for (i=1; i<NumPorts+1; i++)
    {
        Pin = GetPortAddress(i);

        if(CueOffTime[i] > 0)
		{
			if(CueOffTime[i] < ShowTimeMS)
            {
                PinLOW(Pin);
                CueOffTime[i] = 0;
            }
            else
            {
				APinIsOn = 1;
                AnyPinOn = 1;
            }
		}
    }
} 


void Fire(int Port)
{

    int  PortAddress;

    char TraceMsg[30];


    // Translate Port number to hardware address
    PortAddress = GetPortAddress(Port);


    PinHIGH(PortAddress);

    ShowTimeMS = GetTimeMS();
    CueOffTime[Port] = ShowTimeMS + MatchFireTime;   
    AnyPinOn = 1;
    LastFire = Port;
    if (TRC == TRUE)
    {
        sprintf(TraceMsg,"Fire Port %d Port %d at %ld",Cue,Port,ShowTimeMS);
        Trace(TraceMsg,None); 
    }

}

int MeasureVoltage(void)
{
    unsigned int H;
    unsigned int L;

    unsigned int result;

    OpenADC(ADC_FOSC_RC    &
            ADC_RIGHT_JUST  &
            ADC_20_TAD,
            ADC_REF_VDD_VSS &
            ADC_CH0        &
            ADC_INT_OFF, ADC_1ANA );

    Delay10TCYx(5);
    ConvertADC();
    while (BusyADC());
    H = ADRESH;
    L = ADRESL;
    result =(H << 8 )| L; 
    CloseADC();
    return result;

}

void SetUnitAddr(char Data[3])
{

    // Set Unit Addess into EEPROM
    int i;
    unsigned char C;

    for (i=EEUnitAddress; i<SizeOfUnitAddress; i++)
    {

        Busy_eep();
        C = Data[i];
        Write_b_eep(i,C);
        DelayMS(10);
    }
}

void SetLogicalAddr(char Data[3])
{

    // Set Unit Addess into EEPROM
    int i;
    unsigned char C;

    for (i=0; i < SizeOfUnitAddress; i++)
    {

        Busy_eep();
        C = Data[(i)];
        Write_b_eep(i+ EELogicalAddress ,C);
        DelayMS(10);
    }
}



void SetSessionStatus(unsigned char Status)
{
    // Set Session Status into EEPROM
    Busy_eep();
    Write_b_eep(EESessionStatus,Status);
    DelayMS(10);

}

void SetArmedStatus(unsigned char Status)
{
    // Set Armed Status into EEPROM
    Busy_eep();
    Write_b_eep(EEArmedStatus,Status);
    DelayMS(10);

}

int GetUnitAddr(void)
{
    int i;
    unsigned char C;
    char Msg[] = "Get Unit Address from EEPROM";

    Trace(Msg,None);

    for (i=EEUnitAddress; i< SizeOfUnitAddress; i++)
    {
        Busy_eep();
        C = Read_b_eep(i);
        if (C < 0x30 | C > 0x39)
            return FALSE;
        TempUnit[i] = C;
    }
    memset(ThisUnit,'\0',sizeof(ThisUnit));

    strncpy(ThisUnit,TempUnit,3);
    strncpy(ThisLogicalAddr,TempUnit,3);
    return TRUE;


}

int GetLogicalAddr(void)
{
    int i;
    unsigned char C;
    char Msg[] = "Get Logical Address from EEPROM";

    Trace(Msg,None);

    for (i=EELogicalAddress; i<(EELogicalAddress + SizeOfUnitAddress); i++)
    {
        Busy_eep();
        C = Read_b_eep(i);
        if (C < 0x30 | C > 0x39)
            return FALSE;
        TempUnit[i-EELogicalAddress] = C;
    }
    memset(ThisLogicalAddr,'\0',sizeof(ThisUnit));

    strncpy(ThisLogicalAddr,TempUnit,3);
    return TRUE;


}



int GetDefaultShow(void)
{
    char  C;     
    int   i; 

    char Msg[] = "Get Default Show ID from EEPROM";
    Trace(Msg,None);

    Busy_eep();
    C = Read_b_eep(EEDefaultShow);
    i = (int) C;
    return i;


}

void SetTEventStatus(unsigned char Status)
{
    // Set TEvent Status into EEPROM
    Busy_eep();
    Write_b_eep(EETEventStatus,Status);
    // DelayMS(10);

}

unsigned char GetSessionStatus(void)
{
    unsigned char C;

    Busy_eep();
    C = Read_b_eep(EESessionStatus);
    return C;
}

unsigned char GetArmedStatus(void)
{
    unsigned char C;

    Busy_eep();
    C = Read_b_eep(EEArmedStatus);
    return C;
}

unsigned char GetTEventStatus(void)
{
    unsigned char C;

    Busy_eep();
    C = Read_b_eep(EETEventStatus);
    return C;
}

void ShowAddressAndSoftwareVersion(void)
{

    GetUnitAddr();
    ShowUnitAddress();
    DelayMS(100); 
    for (i=0; i<SoftwareVersion; i++)
    {
        Pin28HIGH();
        DelayMS(100);
        Pin28LOW();
        DelayMS(100);
    }

}

void Arm()
{
    Armed = TRUE;
    SetArmedStatus('A');
    Pin6HIGH();        // Turn On Arming Circuit
    fArm = TRUE;
    ShowTimeMS = 0;
    StartTimer0();
    StartTimer1();
    ClockStarted = TRUE; 
    //ArmPending = FALSE;
}

void ReArm()
{

    Armed = TRUE;
    Pin6HIGH();
    ArmPending = FALSE;
    StartTimer0();
    StartTimer1();
    ClockStarted = TRUE; 

}

void DisArm()
{
    Armed = FALSE;
    SetArmedStatus('D');
    Pin6LOW();        // Turn On Arming Circuit
    fArm = FALSE;
    ShowTimeMS = 0;
    CloseTimer0();
    CloseTimer1();
    ClockStarted = FALSE; 
    ArmPending = FALSE;
    memset((char *)Message,'\0',sizeof(Message));

}

void CheckpointShowStatus(void)
{
    unsigned char C;
    int i;

    // This routine takes approx 400 MS to execute

    // Checkpoint Show Parameters into EEPROM
    Len = &TEventParameterEnd - &TEventParameterBegin;
    // Len +1 to allow for saving and restoration of integers
    for (i=0; i<Len+1; i++)
    {
        EEAddress = EETEventCheckpoint + i;
        C = *(&TEventParameterBegin + i);
        Busy_eep();
        Write_b_eep(EEAddress,C);
        ClrWdt();
    }     
}


void RestoreShowStatus(void)
{
    unsigned char C;
    char *p;
    int i;
    // Restore critical Show Checkpoint Parameters from EEPROM

    Len = &TEventParameterEnd - &TEventParameterBegin;
    memset(&TEventParameterBegin, '\0', Len);
    // intPort contains show slot number

    // Len +1 to allow for saving and restoration of integers
    for (i=0; i<Len+1; i++)
    {

        Busy_eep();
        C = Read_b_eep(EETEventCheckpoint + i);
        p = &TEventParameterBegin  + i;
        *p = C;
        ClrWdt();
    }

}

void ResetAllPorts(void)
{
    int i;
    int Pin;

    for (i=1; i<NumPorts; i++)
    {
        Pin = GetPortAddress(i);
        PinLOW(Pin);
    }
}













