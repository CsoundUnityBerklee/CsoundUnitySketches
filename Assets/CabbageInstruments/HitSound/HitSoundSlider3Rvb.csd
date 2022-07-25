<Cabbage>
form caption("HitSoundSliderRvb") size(400, 300), guiMode("queue") pluginId("def1")

button bounds(30, 22, 80, 40) channel("trigger")
hslider bounds(32, 76, 150, 50) channel("freq") range(40, 4000, 400, 1, 0.001)
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

gaRevL init 0
gaRevR init 0

instr 1
    kTrig chnget "trigger"
    if changed(kTrig) == 1 then
        event "i", "PlaySound", 0, rnd(.64)
    endif
endin

instr PlaySound
    iDry = rnd(.7)
    iRvb = rnd(.4)
    aEnv expon 1, p3, 0.001
    aSigL oscili aEnv, chnget:i("freq")*aEnv+(rnd(1500))
    aSigR oscili aEnv, chnget:i("freq")*(1-aEnv)+(rnd(6000))
    outs aSigL*iDry, aSigR*iDry
    
    vincr gaRevL, aSigL*iRvb
    vincr gaRevR, aSigR*iRvb  
endin

instr Reverb
        denorm gaRevL, gaRevR
aL, aR  freeverb gaRevL, gaRevR, 0.79, 0.25, sr, 0
        outs aL, aR    
        clear gaRevL, gaRevR
endin  

</CsInstruments>
<CsScore>
;causes Csound to run for about 7000 years...
f0 z
;starts instrument 1 and runs it for a week
i1 0 [60*60*24*7] 
i "Reverb" 0 [60*60*24*7] 
</CsScore>
</CsoundSynthesizer>
