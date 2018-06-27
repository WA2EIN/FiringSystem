

// This device defines the following peripheral symbols
#define _ADC_V5
#define _CC_V2
#define _PWM_V2
#define _EAUSART_V4
#define _SPI_V1
#define _I2C_V1
#define _TMR_V2
#define _PTB_V1
#define _ANCOM_V3
#define _MWIRE_V1

typedef union {
	struct { 
		unsigned volatile STKPTR:5;
		unsigned :1;
		unsigned volatile STKUNF:1;
		unsigned volatile STKFUL:1;
	};
} __STKPTRbits_t;
extern volatile __STKPTRbits_t STKPTRbits @ 0xFFC;
typedef union {
	struct { 
		unsigned volatile RBIF:1;
		unsigned volatile INT0IF:1;
		unsigned volatile TMR0IF:1;
		unsigned          RBIE:1;
		unsigned          INT0IE:1;
		unsigned          TMR0IE:1;
		unsigned          PEIE:1;
		unsigned          GIE:1;
	};
	struct { 
		unsigned :6;
		unsigned          GIEL:1;
		unsigned          GIEH:1;
	};
} __INTCONbits_t;
extern          __INTCONbits_t INTCONbits @ 0xFF2;
typedef union {
	struct { 
		unsigned volatile RBIP:1;
		unsigned :1;
		unsigned volatile TMR0IP:1;
		unsigned :1;
		unsigned volatile INTEDG2:1;
		unsigned volatile INTEDG1:1;
		unsigned volatile INTEDG0:1;
		unsigned          RBPU:1;
	};
} __INTCON2bits_t;
extern          __INTCON2bits_t INTCON2bits @ 0xFF1;
typedef union {
	struct { 
		unsigned volatile INT1IF:1;
		unsigned volatile INT2IF:1;
		unsigned :1;
		unsigned          INT1IE:1;
		unsigned volatile INT2IE:1;
		unsigned :1;
		unsigned volatile INT1IP:1;
		unsigned volatile INT2IP:1;
	};
} __INTCON3bits_t;
extern volatile __INTCON3bits_t INTCON3bits @ 0xFF0;
typedef union {
	struct { 
		unsigned volatile CARRY:1;
		unsigned volatile DC:1;
		unsigned volatile ZERO:1;
		unsigned volatile OV:1;
		unsigned volatile NEGATIVE:1;
	};
} __STATUSbits_t;
extern volatile __STATUSbits_t STATUSbits @ 0xFD8;
typedef union {
	struct { 
		unsigned volatile T0PS:3;
		unsigned          PSA:1;
		unsigned          T0SE:1;
		unsigned          T0CS:1;
		unsigned          T08BIT:1;
		unsigned          TMR0ON:1;
	};
} __T0CONbits_t;
extern          __T0CONbits_t T0CONbits @ 0xFD5;
typedef union {
	struct { 
		unsigned volatile SCS:2;
		unsigned volatile IOFS:1;
		unsigned volatile OSTS:1;
		unsigned volatile IRCF:3;
		unsigned          IDLEN:1;
	};
} __OSCCONbits_t;
extern          __OSCCONbits_t OSCCONbits @ 0xFD3;
typedef union {
	struct { 
		unsigned volatile HLVDL:1;
		unsigned volatile HLVDL1:1;
		unsigned volatile HLVDL2:1;
		unsigned volatile HLVDL3:1;
		unsigned          HLVDEN:1;
		unsigned volatile IRVST:2;
		unsigned          VDIRMAG:1;
	};
	struct { 
		unsigned volatile LVDL:1;
		unsigned volatile LVDL1:1;
		unsigned volatile LVDL2:1;
		unsigned volatile LVDL3:1;
		unsigned          LVDEN:1;
	};
} __HLVDCONbits_t;
extern          __HLVDCONbits_t HLVDCONbits @ 0xFD2;
typedef union {
	struct { 
		unsigned volatile SWDTEN:1;
	};
} __WDTCONbits_t;
extern volatile __WDTCONbits_t WDTCONbits @ 0xFD1;
typedef union {
	struct { 
		unsigned volatile BOR:1;
		unsigned volatile POR:1;
		unsigned volatile PD:1;
		unsigned volatile TO:1;
		unsigned volatile RI:1;
		unsigned :1;
		unsigned          SBOREN:1;
		unsigned          IPEN:1;
	};
} __RCONbits_t;
extern          __RCONbits_t RCONbits @ 0xFD0;
typedef union {
	struct { 
		unsigned          TMR1ON:1;
		unsigned          TMR1CS:1;
		unsigned          T1SYNC:1;
		unsigned          T1OSCEN:1;
		unsigned volatile T1CKPS:2;
		unsigned          T1RUN:1;
		unsigned volatile T1RD16:1;
	};
} __T1CONbits_t;
extern volatile __T1CONbits_t T1CONbits @ 0xFCD;
typedef union {
	struct { 
		unsigned volatile T2CKPS:2;
		unsigned          TMR2ON:1;
		unsigned volatile T2OUTPS:4;
	};
} __T2CONbits_t;
extern volatile __T2CONbits_t T2CONbits @ 0xFCA;
typedef union {
	struct { 
		unsigned volatile BF:1;
		unsigned volatile UA:1;
		unsigned volatile RW:1;
		unsigned volatile START:1;
		unsigned volatile STOP:1;
		unsigned volatile DA:1;
		unsigned volatile CKE:1;
		unsigned volatile SMP:1;
	};
} __SSPSTATbits_t;
extern volatile __SSPSTATbits_t SSPSTATbits @ 0xFC7;
typedef union {
	struct { 
		unsigned volatile SSPM:4;
		unsigned volatile CKP:1;
		unsigned volatile SSPEN:1;
		unsigned volatile SSPOV:1;
		unsigned volatile WCOL:1;
	};
} __SSPCON1bits_t;
extern volatile __SSPCON1bits_t SSPCON1bits @ 0xFC6;
typedef union {
	struct { 
		unsigned volatile SEN:1;
		unsigned volatile RSEN:1;
		unsigned volatile PEN:1;
		unsigned volatile RCEN:1;
		unsigned volatile ACKEN:1;
		unsigned volatile ACKDT:1;
		unsigned volatile ACKSTAT:1;
		unsigned volatile GCEN:1;
	};
} __SSPCON2bits_t;
extern volatile __SSPCON2bits_t SSPCON2bits @ 0xFC5;
typedef union {
	struct { 
		unsigned          ADON:1;
		unsigned volatile GODONE:1;
		unsigned volatile CHS:4;
	};
} __ADCON0bits_t;
extern volatile __ADCON0bits_t ADCON0bits @ 0xFC2;
typedef union {
	struct { 
		unsigned volatile PCFG:4;
		unsigned volatile VCFG:2;
	};
} __ADCON1bits_t;
extern volatile __ADCON1bits_t ADCON1bits @ 0xFC1;
typedef union {
	struct { 
		unsigned volatile ADCS:3;
		unsigned volatile ACQT:3;
		unsigned :1;
		unsigned          ADFM:1;
	};
} __ADCON2bits_t;
extern          __ADCON2bits_t ADCON2bits @ 0xFC0;
typedef union {
	struct { 
		unsigned volatile CCP1M:4;
		unsigned volatile DC1B:2;
		unsigned          P1M:2;
	};
} __CCP1CONbits_t;
extern          __CCP1CONbits_t CCP1CONbits @ 0xFBD;
typedef union {
	struct { 
		unsigned volatile CCP2M:4;
		unsigned volatile DC2B:2;
	};
} __CCP2CONbits_t;
extern volatile __CCP2CONbits_t CCP2CONbits @ 0xFBA;
typedef union {
	struct { 
		unsigned volatile ABDEN:1;
		unsigned volatile WUE:1;
		unsigned :1;
		unsigned volatile BRG16:1;
		unsigned volatile SCKP:1;
		unsigned volatile RXCKP:1;
		unsigned volatile RCIDL:1;
		unsigned volatile ABDOVF:1;
	};
	struct { 
		unsigned :4;
		unsigned volatile TXCKP:1;
	};
} __BAUDCONbits_t;
extern volatile __BAUDCONbits_t BAUDCONbits @ 0xFB8;
typedef union {
	struct { 
		unsigned volatile CVR:4;
		unsigned          CVRSS:1;
		unsigned          CVRR:1;
		unsigned          CVROE:1;
		unsigned          CVREN:1;
	};
} __CVRCONbits_t;
extern          __CVRCONbits_t CVRCONbits @ 0xFB5;
typedef union {
	struct { 
		unsigned volatile CM:3;
		unsigned          CIS:1;
		unsigned          C1INV:1;
		unsigned          C2INV:1;
		unsigned volatile C1OUT:1;
		unsigned volatile C2OUT:1;
	};
} __CMCONbits_t;
extern volatile __CMCONbits_t CMCONbits @ 0xFB4;
typedef union {
	struct { 
		unsigned          TMR3ON:1;
		unsigned          TMR3CS:1;
		unsigned          T3SYNC:1;
		unsigned volatile T3CCP1:1;
		unsigned volatile T3CKPS:2;
		unsigned volatile T3CCP2:1;
		unsigned volatile T3RD16:1;
	};
} __T3CONbits_t;
extern volatile __T3CONbits_t T3CONbits @ 0xFB1;
typedef union {
	struct { 
		unsigned volatile TX9D:1;
		unsigned volatile TRMT:1;
		unsigned volatile BRGH:1;
		unsigned volatile SENDB:1;
		unsigned volatile SYNC:1;
		unsigned volatile TXEN:1;
		unsigned volatile TX9:1;
		unsigned volatile CSRC:1;
	};
} __TXSTAbits_t;
extern volatile __TXSTAbits_t TXSTAbits @ 0xFAC;
typedef union {
	struct { 
		unsigned volatile RX9D:1;
		unsigned volatile OERR:1;
		unsigned volatile FERR:1;
		unsigned          ADDEN:1;
		unsigned          CREN:1;
		unsigned          SREN:1;
		unsigned volatile RX9:1;
		unsigned          SPEN:1;
	};
} __RCSTAbits_t;
extern          __RCSTAbits_t RCSTAbits @ 0xFAB;
typedef union {
	struct { 
		unsigned volatile CCP2IP:1;
		unsigned          TMR3IP:1;
		unsigned volatile HLVDIP:1;
		unsigned          BCLIP:1;
		unsigned :2;
		unsigned          CMIP:1;
		unsigned          OSCFIP:1;
	};
	struct { 
		unsigned :2;
		unsigned          LVDIP:1;
	};
} __IPR2bits_t;
extern          __IPR2bits_t IPR2bits @ 0xFA2;
typedef union {
	struct { 
		unsigned volatile CCP2IF:1;
		unsigned volatile TMR3IF:1;
		unsigned volatile HLVDIF:1;
		unsigned volatile BCLIF:1;
		unsigned :2;
		unsigned volatile CMIF:1;
		unsigned volatile OSCFIF:1;
	};
	struct { 
		unsigned :2;
		unsigned volatile LVDIF:1;
	};
} __PIR2bits_t;
extern volatile __PIR2bits_t PIR2bits @ 0xFA1;
typedef union {
	struct { 
		unsigned volatile CCP2IE:1;
		unsigned          TMR3IE:1;
		unsigned volatile HLVDIE:1;
		unsigned          BCLIE:1;
		unsigned :2;
		unsigned          CMIE:1;
		unsigned          OSCFIE:1;
	};
	struct { 
		unsigned :2;
		unsigned          LVDIE:1;
	};
} __PIE2bits_t;
extern          __PIE2bits_t PIE2bits @ 0xFA0;
typedef union {
	struct { 
		unsigned          TMR1IP:1;
		unsigned          TMR2IP:1;
		unsigned          CCP1IP:1;
		unsigned          SSPIP:1;
		unsigned          TXIP:1;
		unsigned          RCIP:1;
		unsigned volatile ADIP:1;
		unsigned          PSPIP:1;
	};
} __IPR1bits_t;
extern          __IPR1bits_t IPR1bits @ 0xF9F;
typedef union {
	struct { 
		unsigned volatile TMR1IF:1;
		unsigned volatile TMR2IF:1;
		unsigned volatile CCP1IF:1;
		unsigned volatile SSPIF:1;
		unsigned volatile TXIF:1;
		unsigned volatile RCIF:1;
		unsigned volatile ADIF:1;
		unsigned volatile PSPIF:1;
	};
} __PIR1bits_t;
extern volatile __PIR1bits_t PIR1bits @ 0xF9E;
typedef union {
	struct { 
		unsigned          TMR1IE:1;
		unsigned          TMR2IE:1;
		unsigned          CCP1IE:1;
		unsigned          SSPIE:1;
		unsigned          TXIE:1;
		unsigned          RCIE:1;
		unsigned volatile ADIE:1;
		unsigned          PSPIE:1;
	};
} __PIE1bits_t;
extern          __PIE1bits_t PIE1bits @ 0xF9D;
typedef union {
	struct { 
		unsigned volatile TUN:5;
		unsigned :1;
		unsigned volatile PLLEN:1;
		unsigned volatile INTSRC:1;
	};
} __OSCTUNEbits_t;
extern volatile __OSCTUNEbits_t OSCTUNEbits @ 0xF9B;
typedef union {
	struct { 
		unsigned volatile TRISC:8;
	};
} __TRISCbits_t;
extern volatile __TRISCbits_t TRISCbits @ 0xF94;
typedef union {
	struct { 
		unsigned volatile TRISB:8;
	};
} __TRISBbits_t;
extern volatile __TRISBbits_t TRISBbits @ 0xF93;
typedef union {
	struct { 
		unsigned volatile TRISA:8;
	};
} __TRISAbits_t;
extern volatile __TRISAbits_t TRISAbits @ 0xF92;
typedef union {
	struct { 
		unsigned volatile LATC:8;
	};
} __LATCbits_t;
extern volatile __LATCbits_t LATCbits @ 0xF8B;
typedef union {
	struct { 
		unsigned volatile LATB:8;
	};
} __LATBbits_t;
extern volatile __LATBbits_t LATBbits @ 0xF8A;
typedef union {
	struct { 
		unsigned volatile LATA:8;
	};
} __LATAbits_t;
extern volatile __LATAbits_t LATAbits @ 0xF89;
typedef union {
	struct { 
		unsigned :3;
		unsigned volatile RE3:1;
	};
} __PORTEbits_t;
extern volatile __PORTEbits_t PORTEbits @ 0xF84;
typedef union {
	struct { 
		unsigned volatile RC:8;
	};
} __PORTCbits_t;
extern volatile __PORTCbits_t PORTCbits @ 0xF82;
typedef union {
	struct { 
		unsigned volatile RB:8;
	};
} __PORTBbits_t;
extern volatile __PORTBbits_t PORTBbits @ 0xF81;
typedef union {
	struct { 
		unsigned volatile RA:8;
	};
} __PORTAbits_t;
extern volatile __PORTAbits_t PORTAbits @ 0xF80;
