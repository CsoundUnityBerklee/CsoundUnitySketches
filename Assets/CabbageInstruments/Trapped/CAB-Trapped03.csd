<Cabbage> bounds(0, 0, 0, 0)
form caption("Trapped03") size(650, 400), guiMode("queue") pluginId("def1")

button  bounds(212, 122, 86, 55) channel("trigger") text("Trigger") textColour("white")
hslider bounds(8, 122, 175, 50) channel("dur") range(4, 12, 6, 1, 0.001) text("Dur") textColour("white")

hslider bounds(178, 8, 158, 50) channel("masterLvl") range(0, 1, 0.6, 1, 0.001) text("Master Lvl") textColour("white")
hslider bounds(336, 8, 150, 50) channel("amp") range(0, 1, 0.6, 1, 0.001) text("Synth Lvl") textColour(255, 255, 255, 255)

hslider bounds(90, 62, 160, 52) channel("frq") range(30,1000, 800, 1, 0.001) text("Frq") textColour("white")
hslider bounds(262, 62, 160, 52) channel("rndFrq") range(2, 100, 100, 1, 0.001) text("RandFrq") textColour("white")
hslider bounds(432, 62, 160, 52) channel("gliss") range(.25, 4, .9, 1, 0.001) text("FrqSweep") textColour("white")

hslider bounds(146, 194, 151, 48) channel("masterRate") range(.15, 8, 1, 1, 0.001) text("MasterRate") textColour("white")
hslider bounds(70, 250, 159, 50) channel("rndRate1") range(10, 100, 20, 1, 0.001) text("RndRate1") textColour("white")
hslider bounds(244, 250, 150, 50) channel("rndRate2") range(10, 100, 30, 1, 0.001) text("RndRate2") textColour("white")
hslider bounds(408, 254, 150, 50) channel("rndRate3") range(10, 100, 40, 1, 0.001) text("RndRate3") textColour("white")

hslider bounds(310, 194, 151, 48) channel("masterDepth") range(.15, 8, 1, 1, 0.001) text("MasterDepth") textColour("white")
hslider bounds(70, 308, 159, 50) channel("rndDep1") range(1, 150, 123, 1, 0.001) text("RndDpth1") textColour("white")
hslider bounds(244, 308, 150, 50) channel("rndDep2") range(5, 260, 234, 1, 0.001) text("RndDpth2") textColour("white")
hslider bounds(406, 308, 150, 50) channel("rndDep3") range(6, 390, 145, 1, 0.001) text("RndDpth3") textColour("white")

hslider bounds(316, 122, 161, 50) channel("rvbSend") range(0, .7, .24, 1, 0.001) text("Rvb Send") textColour("white")
hslider bounds(486, 122, 150, 50) channel("rvbPan") range(.1, 24, 10, 1, 0.001) text("Rvb Pan") textColour("white")

combobox bounds(76, 28, 100, 25), populate("*.snaps"), channelType("string") automatable(0) channel("combo31")  value("1")
filebutton bounds(12, 28, 60, 25), text("Save", "Save"), populate("*.snaps", "test"), mode("named preset") channel("filebutton32")
filebutton bounds(12, 56, 60, 25), text("Remove", "Remove"), populate("*.snaps", "test"), mode("remove preset") channel("filebutton33")
</Cabbage>

<CsoundSynthesizer>

<CsOptions>
-n -dm0
</CsOptions>

<CsInstruments>
ksmps = 64
nchnls = 2
0dbfs = 1

garvb  init  0

giFn01 ftgen 1, 0, 8192, 10, 1
giFn16 ftgen 16, 0, 2048, 9, 1, 3, 0, 3, 1, 0, 6, 1, 0


instr 1
    iDur = chnget:i("dur")
 
    iFrq = chnget:i("frq") + rnd(chnget:i("rndFrq"))
    iAmp = chnget:i("amp")
    kTrig chnget "trigger"
    if changed(kTrig) == 1 then
        event "i", "Trapped03", 0, iDur, iFrq, iAmp 
    endif
endin


instr     Trapped03

iDur       = chnget:i("dur")
iAmp       = chnget:i("amp") 
iAmp       = iAmp * .3            
kFrq       = chnget:k("frq") + rnd(chnget:i("rndFrq"))

kMstrRate  = chnget:k("masterRate")
iRndRate1  = chnget:i("rndRate1")
iRndRate2  = chnget:i("rndRate2")
iRndRate3  = chnget:i("rndRate3")
kMstrDpth  = chnget:k("masterDepth")
iRndDep1   = chnget:i("rndDep1")
iRndDep2   = chnget:i("rndDep2")
iRndDep3   = chnget:i("rndDep3")

iGlis      = chnget:i("gliss")

kRvbSnd    = chnget:k("rvbSend")                                                                                                                                                                             

kRndDp1        transeg    10, iDur, iRndDep1, 1
kRate1         transeg    20, iDur, iRndRate1, 1

kRndDp2        transeg    20, iDur, iRndDep2, 2 
kRate2         transeg    80, iDur, iRndRate2, 2
           
kRndDp3        transeg    40, iDur, iRndDep3, 3   
kRate3         transeg    50, iDur, iRndRate3, 3

kGlis          transeg    1, iDur, 0, iGlis

kEnv1          transeg   0, iDur * .2, 0, iAmp, iDur * .8, 0, 0
kRndH1         randh     kRndDp1 * kMstrDpth, kRate1 * kMstrRate, .5
aSig1          buzz      kEnv1, (kFrq + kRndH1) * kGlis, kRndH1, 1

kEnv2          transeg   0, iDur * .4, 0, iAmp, iDur * .6, 0, 0
kRndH2         randh     kRndDp2 * kMstrDpth, kRate2 * kMstrRate
aSig2          buzz      kEnv2 , ((kFrq * 1.1) + kRndH2) * kGlis, kRndH2, 1

kEnv3          transeg   0, iDur * .6, 0, iAmp, iDur * .4, 0, 0
kRndH3         randh     kRndDp3 * kMstrDpth, kRate3 * kMstrRate, .2
aSig3          buzz      kEnv3, ((kFrq * .99) + kRndH3) * kGlis, kRndH3, 1

kgate    transeg  1, iDur, 0, 1

aL = (aSig2 + aSig3) * kgate
aR = (aSig1 + aSig2) * kgate


amix           =        (aL + aR)

             outs      aL * chnget:k("masterLvl"), aR * chnget:k("masterLvl")
             
garvb          =         garvb + (amix * kRvbSnd) 
 
               endin

               instr     Reverb 
               denorm    garvb
iRvbTime       =         4 
kgate          transeg   1, chnget:i("iDur") + iRvbTime, 0, 0
kSpeed         transeg   1, chnget:i("iDur") * .6, 0, 1, chnget:i("iDur") * .4, 0, 3                   
k1             oscil     .5, chnget:k("rvbPan") * kSpeed, 1
k2             =         .5 + k1
k3             =         1 - k2
asig           reverb    garvb, iRvbTime
               outs      .5 * (asig * k2) * chnget:k("masterLvl"), .5 * ((asig * k3) * (-1)) * chnget:k("masterLvl")
garvb          =         0
               endin

</CsInstruments>

<CsScore>
f0 z

i1 0 [60*60*24*7] 
i "Reverb" 0 [60*60*24*7]  
</CsScore>

</CsoundSynthesizer>