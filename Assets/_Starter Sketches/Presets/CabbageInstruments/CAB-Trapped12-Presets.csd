<Cabbage> bounds(0, 0, 0, 0)
form caption("Trapped12") size(700, 330), guiMode("queue") pluginId("def1")

button  bounds(56, 132, 80, 42) channel("trigger1") text("Trigger1") textColour("white")

checkbox bounds(138, 182, 27, 25), channel("reTrigger"), fontColour("white"),  value(0)
label    bounds(6, 182, 114, 22), text("ReTrigger"), fontColour(255, 255, 255, 255) channel("label9")

hslider bounds(16, 72, 175, 50) channel("dur") range(.01, 6, 2.4, 1, 0.001) text("Dur") textColour("white")

hslider bounds(4, 228, 175, 50) channel("rate") range(.02, 20, 8, 1, 0.001) text("ReTrig Rate") textColour("white")

hslider bounds(20, 10, 158, 50) channel("masterLvl") range(0, 1, 0.7, 1, 0.001) text("Master Lvl") textColour("white")

hslider bounds(206, 8, 150, 50) channel("synthLvl") range(0, 1, 0.7, 1, 0.001) text("Synth Lvl") textColour(255, 255, 255, 255)
hslider bounds(362, 10, 150, 50) channel("noiseLvl") range(0, 1, 0.2, 1, 0.001) text("Noise Lvl") textColour(255, 255, 255, 255)
hslider bounds(520, 8, 150, 50) channel("verbLvl") range(0, 1, 0.5, 1, 0.001) text("Verb Lvl") textColour("white")

hslider bounds(252, 68, 160, 52) channel("frq") range(200, 4000, 1300, 1, 0.001) text("Freq") textColour("white")
hslider bounds(428, 68, 150, 50) channel("rndFrq") range(1, 4, 2, 1, 0.001) text("RandFreq") textColour("white")

hslider bounds(200, 130, 161, 51) channel("fltStrt") range(80, 8000, 1200, 1, 0.001) text("FilterStart") textColour("white")
hslider bounds(360, 128, 161, 51) channel("fltPeak") range(80, 8000, 2200, 1, 0.001) text("FilterPeak") textColour("white")
hslider bounds(520, 130, 151, 50) channel("fltBW") range(.01, .5, .07, 1, 0.001) text("FilterBW") textColour("white")

hslider bounds(252, 186, 161, 52) channel("rvbSend") range(0, 1, .15, 1, 0.001) text("Rvb Send") textColour("white")
hslider bounds(426, 188, 150, 50) channel("rvbPan") range(.001, 16, 11, 1, 0.001) text("Rvb Pan") textColour("white")

combobox bounds(382, 254, 100, 25), populate("*.snaps"), channelType("string") automatable(0) channel("combo31")  value("1")
filebutton bounds(316, 254, 60, 25), text("Save", "Save"), populate("*.snaps", "test"), mode("named preset") channel("filebutton32")
filebutton bounds(316, 282, 60, 25), text("Remove", "Remove"), populate("*.snaps", "test"), mode("remove preset") channel("filebutton33")
</Cabbage>

<CsoundSynthesizer>

<CsOptions>
-n -dm0
</CsOptions>

<CsInstruments>
ksmps  = 32
nchnls = 2
0dbfs  = 1
 
garvb  init  0

giFun01 ftgen 01, 0, 8192, 10, 1
giFun22 ftgen 22, 0,    8, -2, .001, .004, .007, .003, .002, .005, .009, .006

instr 1

    kTrig   chnget "trigger1"
    kReTrig chnget "reTrigger"
       
    iDur = chnget:i("dur")
    iAmp = chnget:i("synthLvl")
    iFrq = chnget:i("frq")*rnd(chnget:i("rndFrq"))
        
    if kReTrig == 1 then
        kTrig metro chnget:k("rate")
    endif

    if changed(kTrig) == 1 then
        event "i", "Trapped12", 0, iDur, iFrq, iAmp 
    endif
   
endin

               instr     Trapped12
                      
iDur           =         chnget:i("dur")
iAmp           =         chnget:i("synthLvl")             
iFrq           =         chnget:i("frq")*rnd(chnget:i("rndFrq"))

iFltStrt       =         chnget:i("fltStrt")+rnd(30)
iFltPeak       =         chnget:i("fltPeak")+rnd(100)
iFltBW         =         chnget:i("fltBW")+rnd(.1)                                                                                        
                    
ifuncl         =         8                             
                                             
k1             linseg    0, iDur * .8, 8, iDur * .2, 0     
k2             phasor    k1                         
k3             table     k2 * ifuncl, 22                    

kGate          transeg   1, iDur, 0, 0
kCut           transeg   iFltStrt, iDur * .7, 1, iFltPeak, iDur * .3, iFltStrt * .9, 6
;kCut           expseg    iFltStrt, iDur * .7, iFltPeak, iDur * .3, iFltStrt * .9    

anoise         rand      10                     
aFlt           reson     anoise * chnget:k("noiseLvl"), kCut + iFrq, kCut * iFltBW, 1
aSig           oscil     kGate * iAmp, iFrq + k3, 1
aMix           =         kGate * (aSig + aFlt)
               outs      aMix * chnget:k("masterLvl"), aMix * chnget:k("masterLvl")            
garvb          =         garvb + (aMix * chnget:k("rvbSend")) 
               endin

               instr     Reverb 
               denorm    garvb                   
k1             oscil     .5, chnget:k("rvbPan"), 1
k2             =         .5 + k1
k3             =         1 - k2
aSig           reverb    garvb, 3.1
               outs      (aSig * k2) * chnget:k("verbLvl") * chnget:k("masterLvl"), ((aSig * k3) * (-1)) * chnget:k("verbLvl") * chnget:k("masterLvl")
garvb          =         0
               endin

</CsInstruments>

<CsScore>
f0 z
i1 0 [60*60*24*7] 
i "Reverb" 0 [60*60*24*7]  
</CsScore>

</CsoundSynthesizer>