<Cabbage>
form caption("HitSound4RvbPan") size(400, 300), guiMode("queue") pluginId("def1")

button bounds(30, 22, 80, 40) channel("trigger")
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
        event "i", "PlaySound", 0, rnd(.2)
    endif
endin

instr PlaySound
    iRvb = rnd(.24)
    iDry = rnd(.7)
    aEnv expon 1, p3, 0.001
    aSigL oscili aEnv, 100+(aEnv)*400-(rnd(844))
    aSigR oscili aEnv, 124+(1-aEnv)*404+(rnd(1104))
    aMix = (aSigL + aSigR) * iDry
    aPanL, aPanR pan2 aMix, abs(rnd(1))
    outs aPanL, aPanR
    
    vincr gaRevL, aPanL*iRvb
    vincr gaRevR, aPanR*iRvb  
endin

instr Reverb
        denorm gaRevL, gaRevR
aL, aR  freeverb gaRevL, gaRevR, 0.89, 0.25, sr, 0
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
