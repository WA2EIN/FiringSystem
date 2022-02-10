  include <P18CXXX.INC>
        
UARTDATA        UDATA
uartdata        RES     1
BitCount        RES     2
MsgReady        RES     2
Buffer          RES     1AA   ; reserve 426 bytes
QHead           RES     2     ; Queue Head Pointer 0-2
QTail           RES     2     ; Queue Tail Pointer 0-2
NQueue          RES     2     ;  Number in Queue
UARTErrorOR     RES     2
UARTErrorFR     RES     2






    	GLOBAL	uartdata, BitCount
        GLOBAL  MsgReady, Buffer, QHead, QTail, NQueue
        GLOBAL  UARTErrorOR, UARTErrorFR 
	
        END
        

