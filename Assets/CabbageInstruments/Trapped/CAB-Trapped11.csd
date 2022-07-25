<Cabbage> bounds(0, 0, 0, 0)
form caption("Trapped11") size(650, 300), guiMode("queue") pluginId("def1")

button  bounds(16, 60, 101, 44) channel("trigger") text("Trigger") textColour("white")

hslider bounds(122, 56, 150, 50) channel("dur") range(3, 8, 5, 1, 0.001) text("Dur") textColour("white")
hslider bounds(328, 66, 150, 50) channel("note") range(23, 98, 60, 1, 0.001) text("MIDI NN") textColour("white")
hslider bounds(486, 66, 150, 50) channel("rndNote") range(0, 8, 3, 1, 0.001) text("Rnd NN") textColour("white")

hslider bounds(454, 8, 150, 50) channel("amp") range(0, 1, .6, 1, 0.001) text("Synth Lvl") textColour("white")

hslider bounds(214, 120, 162, 50) channel("mstrRate") range(10, 4000, 185, 1, 0.001) text("MasterRate") textColour("white")
hslider bounds(384, 120, 150, 50) channel("mstrDpth") range(0, 40, 3, 1, 0.001) text("MasterDepth") textColour("white")

hslider bounds(134, 176, 150, 50) channel("delaySend") range(0, 1, .76, 1, 0.001) text("Delay Send") textColour("white")
hslider bounds(300, 178, 150, 50) channel("loopTime") range(.0005, 3.65, .78, 1, 0.001) text("Delay Time") textColour("white")
hslider bounds(466, 178, 150, 50) channel("feedbasck") range(.5, 5.65, 3, 1, 0.001) text("Delay FeedB") textColour("white")

hslider bounds(208, 238, 150, 50) channel("rvbSend") range(0, 1, .35, 1, 0.001) text("Rvb Send") textColour("white")
hslider bounds(374, 236, 150, 50) channel("rvbPan") range(0, 11, .2, 1, 0.001) text("Rvb Pan") textColour("white")

hslider bounds(294, 8, 150, 50) channel("masterLvl") range(0, 1, 0.7, 1, 0.001) text("Master Lvl") textColour("white")

combobox bounds(90, 126, 100, 25), populate("*.snaps"), channelType("string") automatable(0) channel("combo31") text("sizzle", "burble") value("1")
filebutton bounds(28, 126, 60, 25), text("Save", "Save"), populate("*.snaps", "test"), mode("named preset") channel("filebutton32")
filebutton bounds(28, 156, 60, 25), text("Remove", "Remove"), populate("*.snaps", "test"), mode("remove preset") channel("filebutton33")
</Cabbage>
<CsoundSynthesizer>
<CsOptions>
-n -dm0
</CsOptions>
<CsInstruments>
ksmps = 64
nchnls = 2
0dbfs = 1

garvb          init      0
gadel          init      0

giFn01 ftgen 1, 0, 8192, 10, 1
giFn14 ftgen 14, 0, 512, 9, 1, 3, 0, 3, 1, 0, 9, .333, 180

instr 1
    iDur = chnget:i("dur")
 
    iNote = chnget:i("frq")+rnd(chnget:i("rndNote"))
    iFrq = cpsmidinn(iNote)
    iAmp = chnget:i("amp")
    kTrig chnget "trigger"
    if changed(kTrig) == 1 then
        event "i", "Trapped11", 0, iDur, iFrq, iAmp
    endif
endin


instr  Trapped11

ip3    = chnget:i("dur")
ip6    = chnget:i("amp")*.2
              
kNote  = chnget:k("note")+rnd(chnget:i("rndNote"))
kFrq   = cpsmidinn(kNote)

kDepth = chnget:k("mstrDpth")
kRate = chnget:k("mstrRate")
                                            
k1             expseg    1, ip3 * .5, 40, ip3 * .5, 2    
k2             expseg    10, ip3 * .72, 35, ip3 * .28, 6
k3             linen     ip6, ip3 * .333, ip3, ip3 * .333
k4             randh     k1*kDepth, (k2*kRate)+rnd(12), .5
a4             oscil     k3, kFrq + (kFrq * .05) + k4, 1, .1
     
k5             linseg    .4, ip3 * .9, 26, ip3 * .1, 0
k6             linseg    8, ip3 * .24, 20, ip3 * .76, 2
k7             linen     ip6, ip3 * .5, ip3, ip3 * .46
k8             randh     k5*kDepth, (k6*kRate)+rnd(10), .4
a3             oscil     k7, kFrq + (kFrq * .03) + k8, 14, .3

k9             expseg    1, ip3 * .7, 50, ip3 * .3, 2
k10            expseg    10, ip3 * .3, 45, ip3 * .7, 6
k11            linen     ip6, ip3 * .25, ip3, ip3 * .25
k12            randh     k9*kDepth, (k10*kRate)+rnd(3), .5
a2             oscil     k11, kFrq + (kFrq * .02) + k12, 1, .1

k13            linseg    .4, ip3 * .6, 46, ip3 * .4, 0
k14            linseg    18, ip3 * .1, 50, ip3 * .9, 2
k15            linen     ip6, ip3 * .2, ip3, ip3 * .3
k16            randh     k13*kDepth, (k14*kRate)+rnd(4), .8
a1             oscil     k15, kFrq + (kFrq * .01) + k16, 14, .3

kgate         transeg   1,ip3,0,0
amix           =        (a1 + a2 + a3 + a4) * kgate
aL             =        a1 + a3
aR             =        a2 + a4

              outs      aL * chnget:k("masterLvl"), aR * chnget:k("masterLvl")
                
garvb          =         garvb + (amix * chnget:k("rvbSend")) 
gadel          =         gadel + (amix * chnget:k("delaySend")) 
               
endin

               instr     Delay
               denorm    gadel
kFeedback      =         chnget:k("feedback")
iLoopTime      =         chnget:i("loopTime")
aL             comb      gadel, kFeedback*rnd(1), iLoopTime+rnd(.8)
aR             comb      gadel, kFeedback*rnd(1), iLoopTime+rnd(.5)
               outs      aL*chnget:k("masterLvl"), aR*chnget:k("masterLvl")
gadel          =         0
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
i "Delay" 0 [60*60*24*7]
i "Reverb" 0 [60*60*24*7]  
</CsScore>
</CsoundSynthesizer>
