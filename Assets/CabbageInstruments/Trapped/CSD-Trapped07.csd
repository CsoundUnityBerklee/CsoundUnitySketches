<Cabbage> bounds(0, 0, 0, 0)
form caption("Trapped07") size(700, 330), guiMode("queue") pluginId("def1")

hslider bounds(208, 42, 158, 50) channel("masterLvl") range(0, 1, 0.7, 1, 0.001) text("Master Lvl") textColour("white")
hslider bounds(368, 42, 150, 50) channel("synthLvl") range(0, 1, 0.7, 1, 0.001) text("Synth Lvl") textColour(255, 255, 255, 255)
hslider bounds(514, 42, 150, 50) channel("verbLvl") range(0, 1, 0.5, 1, 0.001) text("Verb Lvl") textColour("white")

hslider bounds(0, 102, 175, 50) channel("dur") range(.001, 23, 14, 1, 0.001) text("Dur") textColour("white")
hslider bounds(176, 102, 175, 50) channel("rndDur") range(.01, 0.3, .3, 1, 0.001) text("RandDur") textColour("white")

hslider bounds(356, 102, 160, 52) channel("frq") range(23, 93, 45, 1, 0.001) text("Freq") textColour("white")
hslider bounds(514, 102, 150, 50) channel("rndFrq") range(1, 101, 32, 1, 0.001) text("RandFreq") textColour("white")

button  bounds(58, 180, 80, 42) channel("trigger1") text("Trigger1") textColour("white")
checkbox bounds(150, 232, 27, 25), channel("reTrigger"), fontColour("white"),  value(0)
label    bounds(18, 232, 114, 22), text("ReTrigger"), fontColour(255, 255, 255, 255) channel("label9")
hslider bounds(18, 264, 175, 50) channel("reTrigRate") range(.02, 20, 8, 1, 0.001) text("ReTrig Rate") textColour("white")

hslider bounds(284, 166, 161, 51) channel("modFrqStrt") range(.01, .99, .12, 1, 0.001) text("ModFrqStart") textColour("white")
hslider bounds(458, 164, 161, 51) channel("modFrqPeak") range(.1, .99, .94, 1, 0.001) text("ModFrqPeak") textColour("white")

hslider bounds(200, 218, 151, 50) channel("ctrlAmp") range(.3, 1, .86, 1, 0.001) text("CtrlAmp") textColour("white")
hslider bounds(350, 218, 151, 50) channel("ctrlFunc") range(2, 4, 2, 1, 1) text("CtrlFunc") textColour("white")
hslider bounds(502, 218, 151, 50) channel("oscFunc") range(2, 4, 3, 1, 1) text("OscFunc") textColour("white")

hslider bounds(284, 270, 161, 52) channel("rvbSend") range(0, 1, .5, 1, 0.001) text("Rvb Send") textColour("white")
hslider bounds(446, 270, 150, 50) channel("rvbPan") range(.001, 1, .01, 1, 0.001) text("Rvb Pan") textColour("white")

combobox bounds(92, 28, 100, 25), populate("*.snaps"), channelType("string") automatable(0) channel("combo31")  value("1")
filebutton bounds(26, 28, 60, 25), text("Save", "Save"), populate("*.snaps", "test"), mode("named preset") channel("filebutton32")
filebutton bounds(26, 56, 60, 25), text("Remove", "Remove"), populate("*.snaps", "test"), mode("remove preset") channel("filebutton33")
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
giFun02 ftgen 02, 0,  512, 10, 10,  8,   0,   6,   0,   4,   0,   1
giFun03 ftgen 03, 0,  512, 10, 10,  0,   5,   5,   0,   4,   3,   0,   1
giFun04 ftgen 04, 0,  512, 09, 1,   3,   0,   3,   1,   0,   9,  .333,  180

instr 1

    kTrig   chnget "trigger1"
    kReTrig chnget "reTrigger"
       
    iDur = chnget:i("dur")
    iAmp = chnget:i("synthLvl")
    iFrq = chnget:i("frq")*rnd(chnget:i("rndFrq"))
        
    if kReTrig == 1 then
        kRndH randh chnget:k("reTrigRate")*.4, 4
        kTrig metro chnget:k("reTrigRate")+kRndH
    endif

    if changed(kTrig) == 1 then
        event "i", "Trapped07", 0, iDur, iFrq, iAmp 
    endif
   
endin

               instr     Trapped07


                         
ip3             =         chnget:i("dur")+rnd(chnget:i("rndDur"))
ip4             =         chnget:i("synthLvl")             
ifreq           =         cpsmidinn(chnget:i("frq"))+cpsmidinn(rnd(chnget:i("rndFrq")))

ip6             =         chnget:i("modFrqStrt")*rnd(.19)
ip7             =         chnget:i("modFrqPeak")*rnd(.99)

kp8            =          chnget:k("ctrlAmp")*rnd(.6)
ip9            =          int(chnget:i("ctrlFunc"))

ip10           =          int(chnget:i("oscFunc"))
                                                                                                                                                                            
ifuncl         =         512                                                                                                   

    kReTrig chnget "reTrigger"
    if kReTrig == 1 then
        ip3 = ip3 *.2
        ip4 = ip4 *.12
    endif
kenv           transeg   0, ip3 * .2, 0, ip4, ip3 * .4, 0, ip4, ip3 * .4, 0, 0   

k1             linseg    ip6, ip3 * .5, ip7, ip3 * .5, ip6       
a3             oscili    kp8, ifreq + k1, ip9            
a4             phasor    ifreq                         
a5             table     (a4 + a3) * ifuncl, ip10 
aL             =         kenv * a5                
aR             oscil     kenv, ifreq + .9, ip10  
                              
aMix           =         aL + aR

               outs      aL * chnget:k("synthLvl") * chnget:k("masterLvl"), aR * chnget:k("synthLvl") * chnget:k("masterLvl") 
                          
garvb          =         garvb + (aMix * chnget:k("rvbSend")) 
               endin

               instr     Reverb 
               denorm    garvb                   
k1             oscil     .5, chnget:k("rvbPan"), 1
k2             =         .5 + k1
k3             =         1 - k2
aSig           reverb    garvb, 2.3
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