  ;include <P18CXXX.INC>
  include "p18F2525.INC"

; Modified P. Cranwell for PIC18F2515 fixed pins 6&7  (Port A, Bits 4&5)

SWTXD           equ     PORTA           ; Transmit pin port and pin
SWTXDpin        equ     2               ; PIC18F2525 Pin 4
TRIS_SWTXD      equ     TRISA           ; Transmit pin tris and pin
;******************************************************************************
;    No Receive Pin for this application
;SWRXD           equ     PORTA           ; Receive pin port and pin
;SWRXDpin        equ     4               / PIC18F2525 Pin 6
;TRIS_SWRXD      equ     TRISA           ; Receive pin tris and pin
;******************************************************************************

        EXTERN  DelayRXHalfBitUART
        EXTERN  DelayRXBitUART
        EXTERN  DelayTXBitUART
        EXTERN  uartdata
        EXTERN  BitCount

UARTCODE        CODE
WriteUART
        banksel uartdata
        ; Retrieve character from stack.
        ; modified by p. cranwell 11/18/14 to address stack properly
        movlw   0xFE
        movff   PLUSW1,uartdata

        movlw   0x09                    ; Set bit counter for 9 bits
        movwf   BitCount
        bcf     STATUS,C        ; Send start bit (carry = 0)
        banksel SWTXD
        goto    SendStart
SendBit
        banksel uartdata
        rrcf    uartdata,F              ; Rotate next bit into carry
SendStart
        banksel SWTXD
        btfss   STATUS,C                ; Set or clear TXD pin according
        bcf     SWTXD,SWTXDpin  ; to what is in the carry
        btfsc   STATUS,C
        bsf     SWTXD,SWTXDpin
        call    DelayTXBitUART          ; Delay for 1 bit time
        banksel BitCount
        decfsz  BitCount,F              ; Count only 9 bits
        goto    SendBit
        banksel SWTXD
        bsf     SWTXD,SWTXDpin  ; Stop bit is high
        call    DelayTXBitUART          ; Delay for stop bit
        return

        GLOBAL  WriteUART

        END
