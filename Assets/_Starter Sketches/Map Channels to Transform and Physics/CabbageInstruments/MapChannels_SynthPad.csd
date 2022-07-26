<Cabbage> bounds(0, 0, 0, 0)
form caption("Untitled") size(800, 500), guiMode("queue"), pluginId("def1")
button bounds(302, 4, 80, 40) channel("trigger")

hslider bounds(4, 2, 150, 50) channel("pitch1") range(0, 128, 50, 1, 1) text("Pitch1")
hslider bounds(4, 42, 150, 50) channel("pitch2") range(0, 128, 57, 1, 1) text("Pitch2")
hslider bounds(4, 82, 150, 50) channel("pitch3") range(0, 128, 66, 1, 1) text("Pitch3")
hslider bounds(6, 122, 150, 50) channel("filter") range(200, 10000, 200, 1, 0.1) text("filter")


hslider bounds(232, 186, 150, 50) channel("volume") range(0, 1, 0.5, 1, 0.001) text("volume")

hslider bounds(6, 184, 150, 50) channel("pulsewidth") range(0.1, 0.9, 0.7, 1, 0.01) text("pulsewidth")
hslider bounds(6, 224, 150, 50) channel("vibfreq") range(1, 5, 1, 1, 0.1) text("vibfreq")
hslider bounds(6, 266, 150, 50) channel("vibamp") range(0.5, 5, 0.5, 1, 0.1) text("vibamp")
hslider bounds(6, 366, 150, 50) channel("distortion") range(0, 0.7, 0, 1, 0.01) text("distortion")



hslider bounds(232, 130, 150, 50) channel("pan") range(0, 1, 0.5, 1, 0.01) text("pan")
hslider bounds(228, 64, 150, 50) channel("mix") range(0, 1, 0.25, 1, 0.01) text("mix")

</Cabbage>
<CsoundSynthesizer>
<CsOptions>
-n -d -+rtmidi=NULL -M0 --midi-key-cps=4 --midi-velocity-amp=5
</CsOptions>
<CsInstruments>
; Initialize the global variables. 
ksmps = 32
nchnls = 2
0dbfs = 1

alwayson "Trigger"

giSine          ftgen   1, 0, 8192, 10, 1
giTriangle      ftgen   2,0, 8192, 9, 1, 1, 0, 3, .333, 180, 5, .2, 0, 7, .143, 180, 9, .111, 0
giDist	        ftgen	10,0, 257, 9, .5,1,270


instr Trigger
    kTrig chnget "trigger"
    kIsOnTrig   changed kTrig
    
    //Turn insturment on indefinitely on triggeer and turn it off when trigger is hit again
    if kTrig == 1 then
        if kIsOnTrig == 1 then
            event "i", "Pad", 0, 60*60*24*7
        endif
    elseif kTrig == 0 then
        turnoff2 "Pad", 0, 1
    endif
endin

;instrument will be triggered by keyboard widget
instr Pad
    kEnv            madsr       3, .1, 1, 3
    kVolume         chnget      "volume"
    kPan            chnget      "pan"
    //Pitch
    kPitch1         chnget      "pitch1"
    kPitch2         chnget      "pitch2"
    kPitch3         chnget      "pitch3"
    //Pw
    kPw             chnget      "pulsewidth"
    kPw             port        kPw, 0.01
    //Vibrato
    kVibFreq        chnget      "vibfreq"
    kVibAmp         chnget      "vibamp"
    kVibrato        vibr        kVibAmp, kVibFreq, 2
    //Signal
    aSig1            vco         0.5 * kEnv, cpsmidinn(kPitch1) + kVibrato, 3 ,kPw, giSine  
    aSig2            vco         0.5 * kEnv, cpsmidinn(kPitch2) + kVibrato, 3 ,kPw, giSine  
    aSig3            vco         0.5 * kEnv, cpsmidinn(kPitch3) + kVibrato, 3 ,kPw, giSine  
    aSigSum          sum         aSig1, aSig2, aSig3
    //FILTER
    kFilterFreq     chnget      "filter"
    kFilterFreq     port        kFilterFreq, 0.01
    aSigSum         moogladder  aSigSum, kPitch3 + kFilterFreq, 0.5
    //DISTORTION
    kDistortion     chnget      "distortion"
    aSigSum         distort     aSigSum, kDistortion, giDist
    //PAN
    aSigL, aSigR    pan2        aSigSum * kVolume, kPan
    //REVERB
    kMix            chnget      "mix"
    aVerbL, aVerbR  reverbsc    aSigL, aSigR, 0.8, 5000
    //OUTPUT
    aMixL           ntrpol      aSigL, aVerbL, kMix
    aMixR           ntrpol      aSigR, aVerbR, kMix
    outs aMixL, aMixR
endin

</CsInstruments>
<CsScore>
;causes Csound to run for about 7000 years...
f0 z

</CsScore>
</CsoundSynthesizer>
