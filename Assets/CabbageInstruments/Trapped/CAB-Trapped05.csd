<Cabbage> bounds(0, 0, 0, 0)
form caption("Trapped05") size(680, 360), guiMode("queue") pluginId("def1")

button  bounds(202, 64, 86, 55) channel("trigger") text("Trigger") textColour("white")
hslider bounds(18, 118, 175, 50) channel("dur") range(2, 10, 5, 1, 0.001) text("Dur") textColour("white")

hslider bounds(20, 4, 158, 50) channel("masterLvl") range(0, 1, 0.7, 1, 0.001) text("Master Lvl") textColour("white")
hslider bounds(382, 60, 150, 50) channel("amp") range(0, 1, 0.7, 1, 0.001) text("Synth Lvl") textColour(255, 255, 255, 255)

hslider bounds(298, 112, 160, 52) channel("frq") range(20, 500, 150, 1, 0.001) text("Freq") textColour("white")
hslider bounds(478, 110, 150, 50) channel("rndFrq") range(1, 5, 2, 1, 0.001) text("RandFreq") textColour("white")

hslider bounds(24, 170, 160, 52) channel("pan") range(.1, 1, 1, 1, 0.001) text("Synth Pan") textColour("white")

hslider bounds(302, 216, 159, 50) channel("modFrq") range(10, 100, 10, 1, 0.001) text("Mod Frq") textColour("white")
hslider bounds(480, 218, 150, 50) channel("modAmp") range(100, 1000, 500, 1, 0.001) text("Mod Amp") textColour("white")
hslider bounds(42, 280, 150, 50) channel("modNdx") range(1, 20, 12, 1, 0.001) text("Mod Indx") textColour("white")

hslider bounds(246, 280, 150, 50) channel("mstrModRat") range(1, 20, 12, 1, 0.001) text("MastrModRate") textColour("white")
hslider bounds(452, 280, 150, 50) channel("mstrModDpth") range(1, 20, 1, 1, 0.001) text("MastrModDpth") textColour("white")

hslider bounds(300, 164, 159, 50) channel("carRat") range(1, 10, 1, 1, 0.001) text("Car Ratio") textColour("white")
hslider bounds(478, 160, 150, 55) channel("modRat") range(1, 10, 1, 1, 0.001) text("Mod Ratio") textColour("white")

hslider bounds(300, 8, 161, 49) channel("rvbSend") range(0, 1, .5, 1, 0.001) text("Rvb Send") textColour("white")
hslider bounds(464, 8, 150, 50) channel("rvbPan") range(.01, 34, 25, 1, 0.001) text("Rvb Pan") textColour("white")

combobox bounds(84, 58, 100, 25), populate("*.snaps"), channelType("string") automatable(0) channel("combo31")  value("1")
filebutton bounds(20, 58, 60, 25), text("Save", "Save"), populate("*.snaps", "test"), mode("named preset") channel("filebutton32")
filebutton bounds(20, 86, 60, 25), text("Remove", "Remove"), populate("*.snaps", "test"), mode("remove preset") channel("filebutton33")
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

giFn1 ftgen 1, 0, 8192, 10, 1

instr 1
    iDur = chnget:i("dur")
 
    iFrq = chnget:i("frq")*rnd(chnget:i("rndFrq"))
    iAmp = chnget:i("amp")
    kTrig chnget "trigger"
    if changed(kTrig) == 1 then
        event "i", "Trapped05", 0, iDur, iFrq, iAmp 
    endif
endin

instr Trapped05

iDur        = chnget:i("dur")
iAmp        = chnget:i("amp")             
iFrq        = chnget:i("frq")*rnd(chnget:i("rndFrq"))
iPan        = chnget:i("pan")
iModFrq     = chnget:i("modFrq")
iModAmp     = chnget:i("modAmp")
iCarRat     = chnget:i("carRat")
iModRat     = chnget:i("modRat")
kModNdx     = chnget:k("modNdx")
kMstrRndDp  = chnget:k("mstrModDpth")
kMastRndRt  = chnget:k("mstrModRat")
                                                                                    
kCarRat        line      iCarRat, iDur, 1                     
kModRat        line      1, iDur, iModRat                      
kModAmp        expon     2, iDur, iModAmp                   
kModFrq        linseg    0, iDur * .8, 8, iDur * .2, 8     
kIndx          randh     kModAmp * kMstrRndDp, kModFrq * kMastRndRt                                        
kMod           oscil     kModAmp, kModFrq, 1, .3    

kEnv           linen     iAmp, .03, iDur, .2     
aSig             foscil    kEnv, iFrq + kMod, kCarRat, kModRat, kIndx, 1

kpan           linseg    int(iPan), iDur * .7, frac(iPan), iDur * .3, int(iPan)

               outs      aSig * kpan * chnget:k("masterLvl"), aSig *  (1 - kpan) * chnget:k("masterLvl")
                                     
garvb          =         garvb + (aSig * chnget:k("rvbSend"))  
               endin

               instr     Reverb 
               denorm    garvb                   
k1             oscil     .5, chnget:k("rvbPan"), 1
k2             =         .5 + k1
k3             =         1 - k2
asig           reverb    garvb, 3.1
               outs      (asig * k2) * chnget:k("masterLvl"), ((asig * k3) * (-1)) * chnget:k("masterLvl")
garvb          =         0
               endin

</CsInstruments>
<CsScore>
f0 z
i1 0 [60*60*24*7] 
i "Reverb" 0 [60*60*24*7]  
</CsScore>
</CsoundSynthesizer>