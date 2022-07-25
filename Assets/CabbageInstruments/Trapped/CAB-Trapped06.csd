<Cabbage> bounds(0, 0, 0, 0)
form caption("Trapped06") size(600, 400), guiMode("queue") pluginId("def1")
button  bounds(62, 22, 86, 55) channel("trigger") text("Trigger") textColour("white")
hslider bounds(202, 26, 175, 50) channel("dur") range(1, 9, 4, 1, 0.001) text("Dur") textColour("white")
hslider bounds(34, 86, 158, 50) channel("masterLvl") range(0, 1, 0.8, 1, 0.001) text("Master Lvl") textColour("white")
hslider bounds(390, 88, 162, 50) channel("verbLvl") range(0, 1, 0.4, 1, 0.001) text("Verb Lvl") textColour("white")
hslider bounds(210, 88, 175, 50) channel("amp") range(0, 1, 0.7, 1, 0.001) text("Synth Lvl") textColour(255, 255, 255, 255)
hslider bounds(138, 254, 160, 52) channel("strtFrq") range(20, 3000, 400, 1, 0.001) text("FltSweepSt") textColour("white")
hslider bounds(300, 254, 150, 50) channel("endFrq") range(22, 4000, 1200, 1, 0.001) text("FltSweepNd") textColour("white")
hslider bounds(200, 198, 150, 50) channel("rndFrq") range(0, 1000, 300, 1, 0.001) text("FltRndStNd") textColour("white")
hslider bounds(358, 196, 151, 50) channel("bw") range(.001, .1, .03, 1, 0.001) text("FltBW") textColour("white")
hslider bounds(216, 310, 151, 48) channel("rate") range(.6, 17, 10, 1, 0.001) text("Rate") textColour("white")
hslider bounds(110, 144, 177, 50) channel("rvbSend") range(0, 1, .8, 1, 0.001) text("Rvb Send") textColour("white")
hslider bounds(294, 142, 160, 50) channel("rvbPan") range(.01, 9, 4, 1, 0.001) text("Rvb Pan") textColour("white")
combobox bounds(454, 26, 100, 25), populate("*.snaps"), channelType("string") automatable(0) channel("combo31")  value("1")
filebutton bounds(390, 26, 60, 25), text("Save", "Save"), populate("*.snaps", "test"), mode("named preset") channel("filebutton32")
filebutton bounds(390, 54, 60, 25), text("Remove", "Remove"), populate("*.snaps", "test"), mode("remove preset") channel("filebutton33")
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
giFn19 ftgen 19, 0,   8,  2, 1, 7, 10, 7, 6, 5, 4, 2

instr 1
    iDur = chnget:i("dur")
    kTrig chnget "trigger"
    if changed(kTrig) == 1 then
        event "i", "Trapped06", 0, iDur
    endif
endin

            instr     Trapped06

iFnLen      = 8  

iDur        = chnget:i("dur")

iAmp        = chnget:i("amp")
              
iStFltFrq   = chnget:i("strtFrq")+rnd(chnget:i("rndFrq"))
iEndFltFrq  = chnget:i("endFrq")+rnd(chnget:i("rndFrq"))

kRate       = chnget:k("rate")  

kBW         = chnget:k("bw")
                                                                                                                                                       
kPhase     phasor    kRate                          
k2         table     kPhase * iFnLen, 19                      
aNoise     rand      .4                        
k3         expon     iStFltFrq, iDur, iEndFltFrq                       
aSig       butterbp  aNoise, k3 * k2, k3 * kBW, 1

               outs      aSig * chnget:k("masterLvl"), aSig * chnget:k("masterLvl")            
gaRvb          =         gaRvb + (aSig * chnget:k("rvbSend")) 
               endin


               instr     Reverb 
               denorm    gaRvb                   
k1             oscil     .5, chnget:k("rvbPan"), 1
k2             =         .5 + k1
k3             =         1 - k2
aSig           reverb    gaRvb, 2.1
               outs      (aSig * k2) * chnget:k("verbLvl") * chnget:k("masterLvl"), ((aSig * k3) * (-1)) * chnget:k("verbLvl") * chnget:k("masterLvl")
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