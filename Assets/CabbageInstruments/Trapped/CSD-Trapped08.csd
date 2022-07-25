<CsoundSynthesizer>
<CsOptions>
</CsOptions>
<CsInstruments>
sr             =         44100
kr             =         4410
ksmps          =         10
nchnls         =         2

garvb          init      0

               ctrlinit  1, 21,64, 22,10, 23,10, 24,10, 25,0, 26,10, 27,64

               instr     1

; p4 = AMP
; p5 = FILTERSWEEP STARTFREQ
; p6 = FILTERSWEEP ENDFREQ
; p7 = BANDWIDTH
; p8 = CPS OF RAND1
; p9 = CPS OF RAND2
; p10 = REVERB SEND FACTOR

ip3            midic7    27, 2, 5
ip4            midic7    21, 0, 10
ifreq          cpsmidi
ifreq          =         ifreq*.25
ip6            midic7    22, 10, 5000        ; HF 
kp7            midic7    23, .1, .04
ip8            midic7    24, .1, 32
ip9            midic7    25, .1, 32
kp10           midic7    26, 0, 1

ifuncl         =         16
                                                  
k1             transeg     ifreq, ip3, 0, ip6                     
k2             transeg     ip8, ip3, 0, ip8 * .93              
k3             phasor    k2                            
k4             table     k3 * ifuncl, 20                    
anoise         rand      8000                          
aflt1          butterbp  anoise, k1, 20 + (k4 * k1 * kp7), 1         

k5             linsegr    ip6 * .9, ip3 * .8, ifreq * 1.4, ip3 * .2, ifreq * 1.4
k6             transeg     ip9 * .97, ip3, 0, ip9
k7             phasor    k6
k8             tablei    k7 * ifuncl, 21
aflt2          butterbp  anoise, k5, 30 + (k8 * k5 * kp7 * .9), 1


k11            linenr     ip4, .015, ip3, .5
a3             =         aflt1 * k11
a5             =         aflt2 * k11

k9             randh     1, k2
aleft          =         ((a3 * k9) * .7) + ((a5 * k9) * .3)     
k10            randh     1, k6
aright         =         ((a3 * k10) * .3)+((a5 * k10) * .7)

kmgate         linsegr   0, .001, 1, 1, 0

               outs      aleft * kmgate, aright * kmgate
garvb          =         garvb + ((a3 * kp10) * kmgate)
               endin


               instr     99                 
k1             oscil     .5, p4, 1
k2             =         .5 + k1
k3             =         1 - k2
asig           reverb2   garvb, 2.6, .1
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
;f2   0  512   10   10  8   0   6   0   4   0   1
;f3   0  512   10   10  0   5   5   0   4   3   0   1
;f4   0  2048  10   10  0   9   0   0   8   0   7   0  4  0  2  0  1
;f5   0  2048  10   5   3   2   1   0
;f6   0  2048  10   8   10  7   4   3   1
;f7   0  2048  10   7   9   11  4   2   0   1   1
;f8   0  2048  10   0   0   0   0   7   0   0   0   0  2  0  0  0  1  1
;f9   0  2048  10   10  9   8   7   6   5   4   3   2  1
;f10  0  2048  10   10  0   9   0   8   0   7   0   6  0  5
;f11  0  2048  10   10  10  9   0   0   0   3   2   0  0  1
;f12  0  2048  10   10  0   0   0   5   0   0   0   0  0  3
;f13  0  2048  10   10  0   0   0   0   3   1
;f14  0  512   9    1   3   0   3   1   0   9  .333   180
;f15  0  8192  9    1   1   90 
;f16  0  2048  9    1   3   0   3   1   0   6   1   0
;f17  0  9     5   .1   8   1
;f18  0  17    5   .1   10  1   6  .4
;f19  0  16    2    1   7   10  7   6   5   4   2   1   1  1  1  1  1  1  1
f20  0  16   -2    0   30  40  45  50  40  30  20  10  5  4  3  2  1  0  0  0
f21  0  16   -2    0   20  15  10  9   8   7   6   5   4  3  2  1  0  0
;f22  0  9    -2   .001 .004 .007 .003 .002 .005 .009 .006

f0   60

i99  0   600    6
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
