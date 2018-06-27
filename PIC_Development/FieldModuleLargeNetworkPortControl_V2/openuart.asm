  include <P18CXXX.INC>

SWTXD           equ     PORTA           ; Transmit pin port and pin
SWTXDpin        equ     2               ; PIC18F2525 Pin 4
TRIS_SWTXD      equ     TRISA           ; Transmit pin tris and pin
SWRXD           equ     PORTA           ; Receive pin port and pin
SWRXDpin        equ     4               ; PIC18F2525 Pin 6
TRIS_SWRXD      equ     TRISA           ; Receive pin tris and pin

        EXTERN  DelayRXHalfBitUART
        EXTERN  DelayRXBitUART
        EXTERN  DelayTXBitUART
        EXTERN  uartdata
        EXTERN  BitCount

UARTCODE        CODE
;********************************************************************
;*      Function Name:  OpenUART                                    *
;*      Return Value:   void                                        *
;*      Parameters:     void                                        *
;*      Description:    This routine configures the I/O pins for    *
;*                      software UART.                              *
;********************************************************************
OpenUART

        banksel SWTXD
        bcf             TRIS_SWTXD,SWTXDpin     ; Make TXD an output
        bsf             SWTXD,SWTXDpin          ; Make TXD high
;***************************************************************************************************
       ; Leave the receive because the p[in us used as a Cue in this application
       ;
       ; banksel SWRXD
       ; bsf             TRIS_SWRXD,SWRXDpin     ; Make RXD an input
       ;bsf             SWRXD,SWRXDpin          ; Make RXD high
;****************************************************************************************************
        return

        GLOBAL  OpenUART
        
        END
