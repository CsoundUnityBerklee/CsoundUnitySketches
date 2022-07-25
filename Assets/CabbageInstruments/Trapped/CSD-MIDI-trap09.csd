<CsoundSynthesizer>
<CsOptions>
</CsOptions>
<CsInstruments>
sr             =         44100
kr             =         441
ksmps          =         100
nchnls         =         2
0dbfs = 1

garvb          init      0
gadel          init      0

               ctrlinit  1, 21,64, 22,100, 23,34, 24,85, 25,18, 26,100

               instr     1

; p4 = DELAY SEND FACTOR
; p5 = FREQ
; p6 = AMP
; p7 = REVERB SEND FACTOR
; p8 = RAND AMP
; p9 = RAND FREQ

icps           cpsmidi

ip3            midic7    26, 2, 8
kp4            midic7    22, 0, 1
ifreq          =         icps * .25
ip6            midic7    21, 0, 1
kp7            midic7    23, 0, 1
kp8            midic7    24, 0, 10
kp9            midic7    25, 100, 400                  
                                              
k2             randh     kp8, kp9, .1                     
k3             randh     kp8 * .98, kp9 * .91, .2             
k4             randh     kp8 * 1.2, kp9 * .96, .3            
k5             randh     kp8 * .9, kp9 * 1.3     

kenv           linen     ampmidi(ip6), ip3 *.2, ip3, ip3 * .8                        

a1             oscil     kenv, ifreq + k2, 1, .2             
a2             oscil     kenv * .91, (ifreq + .004) + k3, 2, .3
a3             oscil     kenv * .85, (ifreq + .006) + k4, 3, .5
a4             oscil     kenv * .95, (ifreq + .009) + k5, 4, .8

amix           =         a1 + a2 + a3 + a4

     kgate     linseg    0, .001, 1, ip3 * .998, 1, .001, 0
     kmgate    linsegr   0, .1, 1, 2, 0
     kg        =         kgate * kmgate
               outs      (a1 + a3) * kg, (a2 + a4) * kg
garvb          =         garvb + ((amix * kp7) * kg)
gadel          =         gadel + ((amix * kp4) * kg)
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
; i9:  p4=delsnd,p5=frq,p6=amp,p7=rvbsnd,p8=rndamp,p9=rndfrq                   ;                                          
; i98: p2=strt,p3=dur                                                          ;
; i99: p4=pancps                                                               ;
;========================= FUNCTIONS ==========================================;
f1   0  8192   10   1
f2   0  8192   10   10  8   0   6   0   4   0   1
f3   0  8192   10   10  0   5   5   0   4   3   0   1
f4   0  8192   10   10  0   9   0   0   8   0   7   0  4  0  2  0  1

f0   60

i98  0   600
i99  0   600     4
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
