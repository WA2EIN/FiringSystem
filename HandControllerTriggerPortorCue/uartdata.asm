  include <P18CXXX.INC>
        
UARTDATA        UDATA
uartdata        RES     1
BitCount        RES     2
Msg1Ready       RES     2
Msg2Ready       RES     2
ClockCtr        RES     4
Buffer1         RES     122
Buffer2         RES     40



    	GLOBAL	uartdata, BitCount
        GLOBAL  Msg1Ready, Msg2Ready, Buffer1, Buffer2, ClockCtr
	
        END
        

