<CsoundSynthesizer>
<CsOptions>
-dm0
</CsOptions>
<CsInstruments>

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
;;;   Mateo Larrea Ferro 'Name of Piece' (2022)                ;;;
;;; - based on an instrument from 'Deepest Red' by John ffitch ;;;
;;; and Richard Boulanger's 'Filtered Noise Study' (2021)      ;;;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;; ;;;;;;;;;;;;;;;;;;;;;;;




sr     =    44100
ksmps  =       32
nchnls =        2
0dbfs  =        1

gaL    init     0
gaR    init     0

       turnon   "verb"

       instr    1
iDur   =        p3
iAmp   =        ampdbfs(p4)
iRvb   =        p11
iAtk   =        p12
iRel   =        p13

kFrq   line     p5, iDur, p6
kBand  line     p7, iDur, p8
kPan   line     p9, iDur, p10
kFade  line     1, iDur, ampdbfs(p14)
kEnv   adsr     iAtk, .1, 1, iRel

aNoise rand     iAmp
aFilt  butbp    aNoise, kFrq, kBand
aSig   =        aFilt * kEnv * kFade
aL,aR  pan2     aSig, kPan
       outs     aL, aR

gaL    +=       aL * iRvb
gaR    +=       aR * iRvb
       endin

       instr    verb
       denorm   gaL, gaR
aL,aR  reverbsc gaL, gaR, .95, sr/2
       outs     aL, aR
       clear    gaL, gaR
       endin

</CsInstruments>
<CsScore>

f1  0  8192  10  1
f2  0   513   5  1  513  .001
f3  0   513   5  1  513  .01

;1 strt   dur   amp cfSt cfNd bwSt bwNd PnSt  PnNd rev Atk   Rel   Fade
i1 000.00 100.00 -60 0354 0144 100  040  0.15  0.9  .62 02.00 03.00 -22

</CsScore>
</CsoundSynthesizer>
<bsbPanel>
 <label>Widgets</label>
 <objectName/>
 <x>0</x>
 <y>120</y>
 <width>867</width>
 <height>855</height>
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
