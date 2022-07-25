<CsoundSynthesizer>
<CsOptions>
</CsOptions>
<CsInstruments>
sr             =         44100
kr             =         441
ksmps          =         100
nchnls         =         2

garvb          init      0

               ctrlinit  1, 7,80, 12,40, 13,70, 14,40, 15,60, 16,90

               instr     1
; p5 = FREQ
; p6 = AMP
; p7 = REVERB SEND FACTOR
; p8 = RAND AMP
; p9 = RAND FREQ

icps           cpsmidi

ip3            midic7    16, 1, 10
ip4            midic7    12, 0, 1
ifreq          =         icps * .5
ip6            midic7    7, 0, 2000
kp7            midic7    15, 0, 1
kp8            midic7    14, 0, 8
kp9            midic7    13, 0, 400                       
                                              
k2             randh     kp8, kp9, .1                     
k3             randh     kp8 * .98, kp9 * .91, .2             
k4             randh     kp8 * 1.2, kp9 * .96, .3            
k5             randh     kp8 * .9, kp9 * 1.3     

kenv           linen     ip6, ip3 *.1, ip3, ip3 * .8                        

a1             oscil     kenv, ifreq + k2, 1, .2             
a2             oscil     kenv * .91, (ifreq + .004) + k3, 2, .3
a3             oscil     kenv * .85, (ifreq + .006) + k4, 3, .5
a4             oscil     kenv * .95, (ifreq + .009) + k5, 4, .8

kmgate         linsegr   0, .01, 1, 1, 0
amix           =         kmgate*(a1 + a2 + a3 + a4)
               outs      (a1 + a3) * kmgate, (a2 + a4) * kmgate
garvb          =         garvb + (amix * kp7)
               endin

               instr     99
k1             oscil     .5, p4, 1
k2             =         .5 + k1
k3             =         1 - k2
asig           reverb2   garvb, 4.1, .7
               outs      asig * k2, (asig * k3) * (-1)
garvb          =         0
               endin

</CsInstruments>
<CsScore>
;==============================================================================;
;==============================================================================;
;                            == TRAPPED IN CONVERT ==                          ;
;                                Richard Boulanger                             ;
;==============================================================================; 
;==============================================================================; 
; i1:  p6=amp,p7=vibrat,p8=glisdeltime (default < 1),p9=frqdrop                ;
; i2:  p6=amp,p7=rvbsnd,p8=lfofrq,p9=num of harmonics,p10=sweeprate            ;
; i3:  p6=amp,p7=rvbsnd,p8=rndfrq                                              ;
; i4:  p6=amp,p7=fltrswp:strtval,p8=fltrswp:endval,p9=bdwth,p10=rvbsnd         ;
; i5:  p6=amp,p7=rvbatn,p8=pan:1.0,p9=carfrq,p10=modfrq,p11=modndx,p12=rndfrq  ;
; i6:  p5=swpfrq:strt,p6=swpfrq:end,p7=bndwth,p8=rvbsnd,p9=amp                 ;
; i7:  p4=amp,p5=frq,p6=strtphse,p7=endphse,p8=ctrlamp(.1-1),p9=ctrlfnc        ;
;             p10=audfnc(f2,f3,f14,p11=rvbsnd                                  ;
; i8:  p4=amp,p5=swpstrt,p6=swpend,p7=bndwt,p8=rnd1:cps,p9=rnd2:cps,p10=rvbsnd ;
; i9:  p4=delsnd,p5=frq,p6=amp,p7=rvbsnd,p8=rndamp,p9=rndfrq                   ;
; i10: p4=0,p5=frq,p6=amp,p7=rvbsnd,p8=rndamp,p9=rndfrq                        ;
; i11: p4=delsnd,p5=frq,p6=amp,p7=rvbsnd                                       ;
; i12: p6=amp,p7=swpstrt,p8=swppeak,p9=bndwth,p10=rvbsnd                       ;
; i13: p6=amp,p7=vibrat,p8=dropfrq                                             ;
; i98: p2=strt,p3=dur                                                          ;
; i99: p4=pancps                                                               ;
;========================= FUNCTIONS ==========================================;
f1   0  8192  10   1
f2   0  512   10   10  8   0   6   0   4   0   1
f3   0  512   10   10  0   5   5   0   4   3   0   1
f4   0  2048  10   10  0   9   0   0   8   0   7   0  4  0  2  0  1
f5   0  2048  10   5   3   2   1   0
f6   0  2048  10   8   10  7   4   3   1
f7   0  2048  10   7   9   11  4   2   0   1   1
f8   0  2048  10   0   0   0   0   7   0   0   0   0  2  0  0  0  1  1
f9   0  2048  10   10  9   8   7   6   5   4   3   2  1
f10  0  2048  10   10  0   9   0   8   0   7   0   6  0  5
f11  0  2048  10   10  10  9   0   0   0   3   2   0  0  1
f12  0  2048  10   10  0   0   0   5   0   0   0   0  0  3
f13  0  2048  10   10  0   0   0   0   3   1
f14  0  512   9    1   3   0   3   1   0   9  .333   180
f15  0  8192  9    1   1   90 
f16  0  2048  9    1   3   0   3   1   0   6   1   0
f17  0  9     5   .1   8   1
f18  0  17    5   .1   10  1   6  .4
f19  0  16    2    1   7   10  7   6   5   4   2   1   1  1  1  1  1  1  1
f20  0  16   -2    0   30  40  45  50  40  30  20  10  5  4  3  2  1  0  0  0
f21  0  16   -2    0   20  15  10  9   8   7   6   5   4  3  2  1  0  0
f22  0  9    -2   .001 .004 .007 .003 .002 .005 .009 .006

f0   60

i99  0   600     .2
e
</CsScore>
</CsoundSynthesizer>
