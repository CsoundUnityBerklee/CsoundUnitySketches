<Cabbage> bounds(0, 0, 0, 0)
form caption("HitSoundSlider3Del") size(400, 300), guiMode("queue") pluginId("def1")

button  bounds(32, 22, 80, 40)   channel("trigger")

hslider bounds(140, 18, 150, 50) channel("freq") range(400, 6000, 5400, 1, 0.001)
hslider bounds(32, 70, 150, 50)  channel("tapl") range(0.1, 1, .26, 1, 0.001)
hslider bounds(32, 124, 150, 50) channel("tapm") range(0.1, 1, .66, 1, 0.001)
hslider bounds(32, 180, 150, 50) channel("tapr") range(0.1, 1, .36, 1, 0.001)

hslider bounds(206, 94, 150, 50) channel("feedback") range(.0001, .97, .65, 1, 0.001)
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

gaDelayL init 0
gaDelayR init 0

giMaxDel init 2


instr 1
    kTrig chnget "trigger"
    if changed(kTrig) == 1 then
        event "i", "PlaySound", 0, rnd(.64)
    endif
endin

instr PlaySound
    
    iDry = rnd(.7)
    iDel = rnd(.4)
  
    giTapL = chnget:i("tapl")
    giTapM = chnget:i("tapm")
    giTapR = chnget:i("tapr")
    giFeedback = chnget:i("feedback")
          
    aEnv expon 1, p3, 0.001
    aSigL oscili aEnv, chnget:i("freq")*aEnv+(rnd(1500))
    aSigR oscili aEnv, chnget:i("freq")*(1-aEnv)+(rnd(6000)) 
     
    outs aSigL*iDry, aSigR*iDry
    
    vincr gaDelayL, aSigL*iDel
    vincr gaDelayR, aSigR*iDel  
endin

instr Delay
        denorm gaDelayL, gaDelayR
iFeedback = rnd(.6)
aBuf1	delayr	giMaxDel
aTapL 	deltap3	giTapL		
aTapM 	deltap3	giTapM		
	delayw	gaDelayL + (aTapL * giFeedback)

aBuf2	delayr	giMaxDel
aTapR 	deltap3 giTapR
	delayw	gaDelayR + (aTapR * giFeedback)
                                          	
	outs aTapL + aTapM, aTapR + aTapM   
    clear gaDelayL, gaDelayR
endin  

</CsInstruments>
<CsScore>
;causes Csound to run for about 7000 years...
f0 z
;starts instrument 1 and runs it for a week
i1 0 [60*60*24*7] 
i "Delay" 0 [60*60*24*7] 
</CsScore>
</CsoundSynthesizer>
