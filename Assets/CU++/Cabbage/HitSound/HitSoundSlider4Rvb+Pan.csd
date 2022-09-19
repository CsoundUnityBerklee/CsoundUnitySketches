<Cabbage>
form caption("HitSoundSlider4RvbPan") size(400, 300), guiMode("queue") pluginId("def1")

button bounds(30, 22, 80, 40) channel("trigger")
hslider bounds(32, 76, 150, 49) channel("freq") range(40, 4000, 400, 1, 0.001) , 
hslider bounds(32, 142, 150, 50) channel("pan") range(0, 1, .5, 1, 0.001)
hslider bounds(32, 208, 150, 50) channel("duration") range(0, 1, .5, 1, 0.001)

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
    kDur  chnget "duration"
    kFreq chnget "freq"
    kPan  chnget "pan"
    
    if changed(kTrig) == 1 then
        event "i", "PlaySound", 0, kDur, kFreq, kPan
    endif
endin

instr PlaySound

    iDryLevel = rnd(.7)
    iRvbSend = rnd(.24)
    
    aEnv expon 1, p3, 0.001
    
    aSigL oscili aEnv, p4*aEnv+(rnd(1500))
    aSigR oscili aEnv, p4*(1-aEnv)+(rnd(6000))
    aMix = (aSigL+aSigR)*iDryLevel
    
    aPanL, aPanR pan2 aMix, p5
    outs aPanL, aPanR
    
    vincr gaRevL, aPanL*iRvbSend
    vincr gaRevR, aPanR*iRvbSend  
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
