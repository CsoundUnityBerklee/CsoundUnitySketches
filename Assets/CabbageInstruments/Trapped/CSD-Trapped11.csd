<CsoundSynthesizer>
<CsOptions>
</CsOptions>
<CsInstruments>
sr             =         44100
kr             =         441
ksmps          =         100
nchnls         =         2


garvb          init      0
gadel          init      0

               ctrlinit  1, 21,60, 22,40, 23,20, 24,65

               instr     1

; p4 = DELAY SEND FACTOR
; p5 = FREQ
; p6 = AMP
; p7 = REVERB SEND FACTOR

ifreq          cpsmidi

ip3            midic7    24, 3, 8
kp4            midic7    22, 0, 1
ip6            midic7    21, 0, 2000
kp7            midic7    23, 0, 1
                                             
k1             expseg    1, ip3 * .5, 40, ip3 * .5, 2    
k2             expseg    10, ip3 * .72, 35, ip3 * .28, 6
k3             linen     ip6, ip3* .333, ip3, ip3 * .333
k4             randh     k1, k2, .5
a4             oscil     k3, ifreq + (ifreq * .05) + k4, 1, .1
     
k5             linseg    .4, ip3 * .9, 26, ip3 * .1, 0
k6             linseg    8, ip3 * .24, 20, ip3 * .76, 2
k7             linen     ip6, ip3 * .5, ip3, ip3 * .46
k8             randh     k5, k6, .4
a3             oscil     k7, ifreq + (ifreq * .03) + k8, 14, .3

k9             expseg    1, ip3 * .7, 50, ip3 * .3, 2
k10            expseg    10, ip3 * .3, 45, ip3 * .7, 6
k11            linen     ip6, ip3 * .25, ip3, ip3 * .25
k12            randh     k9, k10, .5
a2             oscil     k11, ifreq + (ifreq * .02) + k12, 1, .1

k13            linseg    .4, ip3 * .6, 46, ip3 * .4, 0
k14            linseg    18, ip3 * .1, 50, ip3 * .9, 2
k15            linen     ip6, ip3 * .2, ip3, ip3 * .3
k16            randh     k13, k14, .8
a1             oscil     k15, ifreq + (ifreq * .01) + k16, 14, .3

     kmgate    linsegr   0, .1, 1, 2, 0

amix           =         kmgate * (a1 + a2 + a3 + a4)
               outs      (a1 + a3) * kmgate, (a2 + a4) * kmgate
garvb          =         garvb + (amix * kp7)
gadel          =         gadel + (amix * kp4)
               endin

               instr     98
asig           delay     gadel, .08
               outs      asig, asig
gadel          =         0
               endin

               instr     99
k1             oscil     .5, p4, 1
k2             =         .5 + k1
k3             =         1 - k2
asig           reverb    garvb, 2.1
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
/*
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
*/
f14  0  512   9    1   3   0   3   1   0   9  .333   180
/*
f15  0  8192  9    1   1   90 
f16  0  2048  9    1   3   0   3   1   0   6   1   0
f17  0  9     5   .1   8   1
f18  0  17    5   .1   10  1   6  .4
f19  0  16    2    1   7   10  7   6   5   4   2   1   1  1  1  1  1  1  1
f20  0  16   -2    0   30  40  45  50  40  30  20  10  5  4  3  2  1  0  0  0
f21  0  16   -2    0   20  15  10  9   8   7   6   5   4  3  2  1  0  0
f22  0  9    -2   .001 .004 .007 .003 .002 .005 .009 .006
*/

f0   60

i98  0   600
i99  0   600     9
e
</CsScore>
</CsoundSynthesizer>
<bsbPanel>
 <label>Widgets</label>
 <objectName/>
 <x>0</x>
 <y>0</y>
 <width>0</width>
 <height>0</height>
 <visible>true</visible>
 <uuid/>
 <bgcolor mode="nobackground">
  <r>255</r>
  <g>255</g>
  <b>255</b>
 </bgcolor>
</bsbPanel>
<bsbPresets>
</bsbPresets>
