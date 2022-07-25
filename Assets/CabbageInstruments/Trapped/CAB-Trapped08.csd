<Cabbage> bounds(0, 0, 0, 0)
form caption("Trapped08") size(650, 300), guiMode("queue") pluginId("def1")

button  bounds(212, 122, 86, 55) channel("trigger") text("Trigger") textColour("white")
hslider bounds(8, 122, 175, 50) channel("dur") range(2, 9, 3, 1, 0.001) text("Dur") textColour("white")

hslider bounds(178, 8, 158, 50) channel("masterLvl") range(0, 1, 0.7, 1, 0.001) text("Master Lvl") textColour("white")
hslider bounds(486, 8, 150, 50) channel("verbLvl") range(0, 1, 0.5, 1, 0.001) text("Verb Lvl") textColour("white")
hslider bounds(336, 8, 150, 50) channel("amp") range(0, 1, 0.7, 1, 0.001) text("Synth Lvl") textColour(255, 255, 255, 255)

hslider bounds(206, 62, 160, 52) channel("frq") range(20, 3000, 1100, 1, 0.001) text("Freq") textColour("white")
hslider bounds(378, 62, 150, 50) channel("rndFrq") range(1, 4, 2, 1, 0.001) text("RandFreq") textColour("white")

hslider bounds(16, 184, 161, 51) channel("fltRange") range(.5, 2, 1, 1, 0.001) text("FilterPeak") textColour("white")
hslider bounds(182, 184, 161, 51) channel("fltCF") range(10, 5000, 1800, 1, 0.001) text("FilterCF") textColour("white")
hslider bounds(354, 186, 151, 50) channel("fltBW") range(.04, .1, .05, 1, 0.001) text("FilterBW") textColour("white")

hslider bounds(96, 240, 151, 48) channel("masterRate") range(.5, 2, 1, 1, 0.001) text("MasterRate") textColour("white")
hslider bounds(256, 240, 159, 50) channel("rndRate1") range(.1, 32, 13, 1, 0.001) text("RndRate1") textColour("white")
hslider bounds(428, 242, 150, 50) channel("rndRate2") range(.1, 32, 10, 1, 0.001) text("RndRate2") textColour("white")

hslider bounds(316, 122, 161, 50) channel("rvbSend") range(0, 1, .15, 1, 0.001) text("Rvb Send") textColour("white")
hslider bounds(486, 122, 150, 50) channel("rvbPan") range(.001, 9, .9, 1, 0.001) text("Rvb Pan") textColour("white")

combobox bounds(76, 28, 100, 25), populate("*.snaps"), channelType("string") automatable(0) channel("combo31")  value("1")
filebutton bounds(12, 28, 60, 25), text("Save", "Save"), populate("*.snaps", "test"), mode("named preset") channel("filebutton32")
filebutton bounds(12, 56, 60, 25), text("Remove", "Remove"), populate("*.snaps", "test"), mode("remove preset") channel("filebutton33")
</Cabbage>

<CsoundSynthesizer>

<CsOptions>
-n -dm0
</CsOptions>

<CsInstruments>
; Initialize the global variables. 
ksmps = 32
nchnls = 2
0dbfs = 1

garvb  init  0

giSine ftgen 1, 0, 8192, 10, 1
giFun20 ftgen 20, 0, 16, -2, 0, 30, 40, 45, 50, 40, 30, 20, 10, 5, 4, 3, 2, 1, 0, 0, 0
giFun21 ftgen 21, 0, 16, -2, 0, 20, 15, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0, 0

gkRevPan init 6

instr 1
    iDur = chnget:i("dur")
 
    iFrq = chnget:i("frq")*rnd(chnget:i("rndFrq"))
    iAmp = chnget:i("amp")
    kTrig chnget "trigger"
    if changed(kTrig) == 1 then
        event "i", "Trapped08", 0, iDur, iFrq, iAmp 
    endif
endin


            instr     Trapped08

iFunLen     = 16

iDur        = chnget:i("dur")

iAmp        = chnget:i("amp")
              
iFrq        = chnget:i("frq")*rnd(chnget:i("rndFrq"))

kMastrRat   = chnget:k("masterRate")
iRndRate1   = chnget:i("rndRate1")
iRndRate2   = chnget:i("rndRate2")

kFltRange = chnget:k("fltRange")
iFltCF    = chnget:i("fltCF")
kFltBW    = chnget:k("fltBW")                                                                                        

anoise         rand      .4                                                                                                    

k1             transeg   iFrq, iDur, 0, iFltCF                     
k2             transeg   iRndRate1, iDur, 0, iRndRate1 * .93              
k3             phasor    k2 * kMastrRat                           
k4             table     k3 * iFunLen, giFun20                                             
aflt1          butterbp  anoise, k1 * kFltRange, 20 + (k4 * k1 * kFltBW), 1         

k5             linsegr   iFltCF * .9, iDur * .8, iFrq * 1.4, iDur * .2, iFrq * 1.4
k6             transeg   iRndRate2 * .97, iDur, 0, iRndRate2
k7             phasor    k6 * kMastrRat
k8             tablei    k7 * iFunLen, giFun21
aflt2          butterbp  anoise, k5 * kFltRange, 30 + (k8 * k5 * (kFltBW * .9)), 1

k11            linenr    iAmp, .015, iDur, .85
a3             =         aflt1 * k11
a5             =         aflt2 * k11

k9             randh     1, k2
aL          =         ((a3 * k9) * .7) + ((a5 * k9) * .3)     
k10            randh     1, k6
aR         =         ((a3 * k10) * .3) + ((a5 * k10) * .7)

kgate         transeg   1, iDur, 0, 0 
amix           =        (aL + aR) * kgate

             outs      aL * chnget:k("masterLvl"), aR * chnget:k("masterLvl")
             
;garvb          =         garvb + (amix * chnget:k("rvbSend")) 
garvb          =         garvb + (a3 * chnget:k("rvbSend")) ; (optional)
 
               endin

               instr     Reverb 
               denorm    garvb                   
k1             oscil     .5, chnget:k("rvbPan"), 1
k2             =         .5 + k1
k3             =         1 - k2
asig           reverb    garvb, 3.1
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


