<Cabbage> bounds(0, 0, 0, 0)
form caption("Trapped04") size(650, 440), guiMode("queue") pluginId("def1")

button  bounds(218, 102, 86, 55) channel("trigger") text("Trigger") textColour("white")
hslider bounds(318, 102, 175, 50) channel("dur") range(.5, 4, 1, 1, 0.001) text("Dur") textColour("white")

hslider bounds(254, 2, 158, 50) channel("masterLvl") range(0, 1, 0.7, 1, 0.001) text("Master Lvl") textColour("white")
hslider bounds(96, 52, 158, 50) channel("synthNoiseMix") range(0, 1, 0.8, 1, 0.001) text("Syn/Nse Mix") textColour("white")
hslider bounds(254, 52, 160, 50) channel("amp") range(0, 1, 0.5, 1, 0.001) text("Synth Lvl") textColour(255, 255, 255, 255)

hslider bounds(412, 52, 150, 50) channel("rvbLvl") range(0, 1, 0.5, 1, 0.001) text("Verb Lvl") textColour("white")
hslider bounds(98, 376, 161, 50) channel("rvbSend") range(0, 1, .15, 1, 0.001) text("Rvb Send") textColour("white")
hslider bounds(264, 376, 150, 50) channel("rvbPanRate") range(.001, 9, .9, 1, 0.001) text("RvbPanRate") textColour("white")
hslider bounds(420, 376, 150, 50) channel("rvbRndRate") range(1, 3, 1, 1, 0.001) text("RvbRndRate") textColour("white")

hslider bounds(174, 156, 160, 52) channel("carFrq") range(20, 3000, 1100, 1, 0.001) text("CarFrq") textColour("white")
hslider bounds(334, 156, 159, 52) channel("carRndFrq") range(1, 4, 1, 1, 0.001) text("CarRndFrq") textColour("white")

hslider bounds(96, 214, 160, 52) channel("modFrq") range(1, 3000, 13, 1, 0.001) text("ModFrq") textColour("white")
hslider bounds(258, 214, 150, 50) channel("modRndFrq") range(1, 4, 1, 1, 0.001) text("ModRndFrq") textColour("white")
hslider bounds(434, 212, 150, 50) channel("modAmp") range(.2, 500, .6, 1, 0.001) text("ModAmp") textColour("white")

hslider bounds(112, 270, 161, 51) channel("fltSweepStart") range(40, 7000, 3000, 1, 0.001) text("FltSwpSt") textColour("white")
hslider bounds(276, 270, 161, 51) channel("fltRndStart") range(1, 2, 1, 1, 0.001) text("FltRndSt") textColour("white")
hslider bounds(112, 320, 161, 51) channel("fltSweepEnd") range(40, 7000, 1000, 1, 0.001) text("FltSwpNd") textColour("white")
hslider bounds(276, 320, 161, 51) channel("fltRndEnd") range(1, 2, 1, 1, 0.001) text("FltRndNd") textColour("white")
hslider bounds(446, 296, 151, 50) channel("fltBW") range(20, 40, 35, 1, 0.001) text("FltBW") textColour("white")

combobox bounds(114, 118, 100, 25), populate("*.snaps"), channelType("string") automatable(0) channel("combo31")  value("1")
filebutton bounds(50, 118, 60, 25), text("Save", "Save"), populate("*.snaps", "test"), mode("named preset") channel("filebutton32")
filebutton bounds(50, 146, 60, 25), text("Remove", "Remove"), populate("*.snaps", "test"), mode("remove preset") channel("filebutton33")
</Cabbage>

<CsoundSynthesizer>

<CsOptions>
-n -dm0
</CsOptions>

<CsInstruments>
ksmps = 32
nchnls = 2
0dbfs = 1

gaRvb  init  0

giFn01 ftgen  1, 0, 8192, 10, 1
giFn15 ftgen 15, 0, 8192,  9, 1, 1, 90


instr 1   
    kTrig chnget "trigger"
        if changed(kTrig) == 1 then
            event "i", "Trapped04", 0, chnget:i("dur")
        endif
endin


            instr     Trapped04

iDur        = chnget:i("dur")
iAmp        = chnget:i("amp")            
kCarFrq     = chnget:k("carFrq")*rnd(chnget:i("carRndFrq"))
kModFrq     = chnget:k("modFrq")*rnd(chnget:i("modRndFrq"))
kModAmp     = chnget:k("modAmp")*rnd(10)

iFltSwpStrt   = chnget:i("fltSweepStart")*rnd(chnget:i("fltRndStart"))
iFltSwpEnd    = chnget:i("fltSweepEnd")*rnd(chnget:i("fltRndEnd")) 
kFltBW        = chnget:k("fltBW")

kSweep         expon     iFltSwpStrt, iDur, iFltSwpEnd                    
aNoise         rand      .4                         
aFltNoise      reson     aNoise, kSweep, kSweep / kFltBW, 1  
          
aMod           oscil     kModAmp, kModFrq, 1, .1               
kEnv           expon     iAmp, iDur, .001 
aCar           oscil     kEnv, kCarFrq + aMod, 15


;               outs      (aFltNoise * .8) + aCar, (aFltNoise * .6) + (aCar * .7)

                                                                                      
;kgate         transeg   1, iDur, 0, 0 
;amix           =        (aL + aR) * kgate

aMix         ntrpol   aCar, aFltNoise, chnget:k("synthNoiseMix")  ; synth-noise mix   

             outs      aMix * chnget:k("masterLvl"), aMix * chnget:k("masterLvl")
                                               
gaRvb          =         gaRvb + (aMix * chnget:k("rvbSend")) 

               endin

               instr     Reverb 
               denorm    gaRvb                   
k1             oscil     .5, chnget:k("rvbPanRate")*rnd(chnget:k("rvbRndRate")), 1
k2             =         .5 + k1
k3             =         1 - k2
aSig           reverb    gaRvb, 2.5
               outs      (aSig * k2) * chnget:k("rvbLvl") * chnget:k("masterLvl"), ((aSig * k3) * (-1)) * chnget:k("rvbLvl") * chnget:k("masterLvl")
gaRvb          =         0
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

