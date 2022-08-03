<Cabbage>

</Cabbage>
<CsoundSynthesizer>
<CsOptions>
</CsOptions>
<CsInstruments>
; Initialize the global variables. 
ksmps = 32
nchnls = 2
0dbfs = 1

;instrument will be triggered by keyboard widget
instr 1 
kGate chnget "gate"
a1 oscil 0.25, 440
outs a1*kGate, a1*kGate
endin

</CsInstruments>
<CsScore>
i 1 0 600000
</CsScore>
</CsoundSynthesizer>
