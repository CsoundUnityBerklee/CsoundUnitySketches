<Cabbage> bounds(0, 0, 0, 0)
form caption("Trapped10") size(650, 600), guiMode("queue") pluginId("def1")

button  bounds(46, 74, 86, 55) channel("trigger") text("Trigger") textColour("white")
hslider bounds(258, 186, 175, 50) channel("dur") range(1, 10, 4, 1, 0.001) text("Dur") textColour("white")

hslider bounds(258, 6, 158, 50) channel("masterLvl") range(0, 1, 0.5, 1, 0.001) text("Master Lvl") textColour("white")
hslider bounds(346, 64, 150, 50) channel("verbLvl") range(0, 1, 0.6, 1, 0.001) text("Verb Lvl") textColour("white")
hslider bounds(184, 62, 150, 50) channel("amp") range(0, 1, 0.5, 1, 0.001) text("Synth Lvl") textColour(255, 255, 255, 255)

hslider bounds(180, 242, 160, 52) channel("frq") range(20, 1000, 396, 1, 0.001) text("Freq") textColour("white")
hslider bounds(352, 242, 150, 50) channel("rndFrq") range(0, 400, 130, 1, 0.001) text("RandFreq") textColour("white")

hslider bounds(258, 306, 151, 57) channel("masterRate") range(.5, 2, 1, 1, 0.001) text("MasterRate") textColour("white")
hslider bounds(10, 374, 159, 50) channel("rndRate1") range(0, 200, 200, 1, 0.001) text("RndRate1") textColour("white")
hslider bounds(170, 376, 150, 50) channel("rndRate2") range(0, 200, 180, 1, 0.001) text("RndRate2") textColour("white")
hslider bounds(320, 374, 159, 50) channel("rndRate3") range(0, 200, 120, 1, 0.001) text("RndRate3") textColour("white")
hslider bounds(482, 374, 150, 50) channel("rndRate4") range(0, 200, 160, 1, 0.001) text("RndRate4") textColour("white")

hslider bounds(270, 434, 151, 48) channel("masterDepth") range(.5, 2, 1, 1, 0.001) text("MasterDepth") textColour("white")
hslider bounds(14, 492, 159, 50) channel("rndDepth1") range(0, 100, 50, 1, 0.001) text("RndDpth1") textColour("white")
hslider bounds(180, 490, 150, 50) channel("rndDepth2") range(0, 100, 45, 1, 0.001) text("RndDpth2") textColour("white")
hslider bounds(332, 490, 159, 50) channel("rndDepth3") range(0, 100, 40, 1, 0.001) text("RndDpth3") textColour("white")
hslider bounds(496, 490, 150, 50) channel("rndDepth4") range(0, 100, 60, 1, 0.001) text("RndDpth4") textColour("white")

hslider bounds(190, 128, 161, 50) channel("rvbSend") range(0, 1, .45, 1, 0.001) text("Rvb Send") textColour("white")
hslider bounds(360, 128, 150, 50) channel("rvbPan") range(.001, 3, .5, 1, 0.001) text("Rvb Pan") textColour("white")

combobox bounds(78, 138, 100, 25), populate("*.snaps"), channelType("string") automatable(0) channel("combo31")  value("1")
filebutton bounds(14, 138, 60, 25), text("Save", "Save"), populate("*.snaps", "test"), mode("named preset") channel("filebutton32")
filebutton bounds(14, 166, 60, 25), text("Remove", "Remove"), populate("*.snaps", "test"), mode("remove preset") channel("filebutton33")
</Cabbage>

<CsoundSynthesizer>

<CsOptions>
-n -dm0
</CsOptions>

<CsInstruments>

ksmps = 32
nchnls = 2
0dbfs = 1

garvb  init  0

giFun1 ftgen 1, 0, 8192, 10, 1
giFun2 ftgen 2, 0, 8192, 10, 1, .8, 0, .6, 0, .4, 0, .01
giFun3 ftgen 3, 0, 8192, 10, 1,  0, .5, .2, 0, .4, .3, 0, .01
giFun4 ftgen 4, 0, 8192, 10, 1,  0, .9, 0, 0, .8, 0, .7, 0, .4, 0, .2, 0, .01

instr 1
    iDur = chnget:i("dur") 
    iFrq = chnget:i("frq") + rnd(chnget:i("rndFrq"))
    iAmp = chnget:i("amp")
    kTrig chnget "trigger"
    if changed(kTrig) == 1 then
        event "i", "Trapped10", 0, iDur, iFrq, iAmp 
    endif
endin

instr  Trapped10

iDur        = chnget:i("dur")
iAmp        = chnget:i("amp")              
iFrq        = chnget:i("frq") + rnd(chnget:i("rndFrq"))

kMstrRate   = chnget:k("masterRate")
kRndRate1   = chnget:k("rndRate1")
kRndRate2   = chnget:k("rndRate2")
kRndRate3   = chnget:k("rndRate3")
kRndRate4   = chnget:k("rndRate4")

kMstrDpth   = chnget:k("masterDepth")
kRndDpth1   = chnget:k("rndDepth1")
kRndDpth2   = chnget:k("rndDepth2")
kRndDpth3   = chnget:k("rndDepth3")
kRndDpth4   = chnget:k("rndDepth4")
                                                                                                                                             
kRnd1          randh     kMstrDpth * kRndDpth1, kMstrRate * kRndRate1, .1                     
kRnd2          randh     kMstrDpth * kRndDpth2, kMstrRate * kRndRate2, .2             
kRnd3          randh     kMstrDpth * kRndDpth3, kMstrRate * kRndRate3, .3            
kRnd4          randh     kMstrDpth * kRndDpth4, kMstrRate * kRndRate4, .4     

kEnv           linen     iAmp, iDur * .1, iDur, iDur * .8                        

a1             oscil     kEnv, iFrq + kRnd1, 1, .2             
a2             oscil     kEnv * .91, (iFrq + .04) + kRnd2, 2, .3
a3             oscil     kEnv * .85, (iFrq + .06) + kRnd3, 3, .5
a4             oscil     kEnv * .95, (iFrq + .09) + kRnd4, 4, .8

;kgate         transeg   1, iDur, 0, 0 

aL              =  a1 + a3
aR              =  a2 + a4
amix            =  aL + aR

               outs      aL * chnget:k("masterLvl"), aR * chnget:k("masterLvl")             
garvb          =         garvb + (amix * chnget:k("rvbSend")) 
               endin
   
               instr     Reverb 
               denorm    garvb                   
k1             oscil     .5, chnget:k("rvbPan"), 1
k2             =         .5 + k1
k3             =         1 - k2
asig           reverb2   garvb, 4.1, .7
               outs      (asig * k2) * chnget:k("verbLvl") * chnget:k("masterLvl"), ((asig * k3) * (-1)) * chnget:k("verbLvl") * chnget:k("masterLvl")
garvb          =         0
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