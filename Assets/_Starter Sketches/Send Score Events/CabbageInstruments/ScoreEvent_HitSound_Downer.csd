<Cabbage>
form caption("HitSoundSlider2") size(400, 300), guiMode("queue") pluginId("def1")

button bounds(30, 22, 80, 40) channel("trigger")
hslider bounds(32, 76, 150, 50) channel("freq") range(40, 4000, 400, 1, 0.001)
hslider bounds(32, 142, 150, 50) channel("pan") range(0, 1, .5, 1, 0.001)

</Cabbage>
<CsoundSynthesizer>
<CsOptions>
-n -d
</CsOptions>
<CsInstruments>
; Initialize the global variables. 
ksmps = 32
nchnls = 2
0dbfs = 1

instr 1
    kTrig chnget "trigger"
    kFreq chnget "freq"
    if changed(kTrig) == 1 then
        event "i", "PlaySound", 0, .5, kFreq
    endif
endin

instr PlaySound
    aEnv expon 1, p3, 0.001
    aSig oscili aEnv*0.25, p4*aEnv
    aPanL, aPanR pan2 aSig, chnget:i("pan")
    outs aPanL, aPanR
endin

</CsInstruments>
<CsScore>
;causes Csound to run for about 7000 years...
f0 z
;starts instrument 1 and runs it for a week
i1 0 [60*60*24*7] 
</CsScore>
</CsoundSynthesizer>
