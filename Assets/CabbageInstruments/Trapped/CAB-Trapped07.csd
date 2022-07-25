<Cabbage> bounds(0, 0, 0, 0)
form caption("Trapped07") size(700, 330), guiMode("queue") pluginId("def1")

button  bounds(56, 132, 80, 42) channel("trigger1") text("Trigger1") textColour("white")

checkbox bounds(138, 182, 27, 25), channel("reTrigger"), fontColour("white"),  value(0)
label    bounds(6, 182, 114, 22), text("ReTrigger"), fontColour(255, 255, 255, 255) channel("label9")

hslider bounds(16, 72, 175, 50) channel("dur") range(.1, 5, 2.4, 1, 0.001) text("Dur") textColour("white")

hslider bounds(4, 228, 175, 50) channel("rate") range(.02, 20, 8, 1, 0.001) text("ReTrig Rate") textColour("white")

hslider bounds(20, 10, 158, 50) channel("masterLvl") range(0, 1, 0.7, 1, 0.001) text("Master Lvl") textColour("white")

hslider bounds(206, 8, 150, 50) channel("synthLvl") range(0, 1, 0.7, 1, 0.001) text("Synth Lvl") textColour(255, 255, 255, 255)
hslider bounds(362, 10, 150, 50) channel("noiseLvl") range(0, 1, 0.2, 1, 0.001) text("Noise Lvl") textColour(255, 255, 255, 255)
hslider bounds(520, 8, 150, 50) channel("verbLvl") range(0, 1, 0.5, 1, 0.001) text("Verb Lvl") textColour("white")

hslider bounds(252, 68, 160, 52) channel("frq") range(200, 4000, 1300, 1, 0.001) text("Freq") textColour("white")
hslider bounds(428, 68, 150, 50) channel("rndFrq") range(1, 4, 2, 1, 0.001) text("RandFreq") textColour("white")

hslider bounds(200, 130, 161, 51) channel("modFrqStrt") range(.01, .99, .2, 1, 0.001) text("ModFrqStart") textColour("white")
hslider bounds(360, 128, 161, 51) channel("modFrqPeak") range(.1, .99, .4, 1, 0.001) text("ModFrqPeak") textColour("white")

hslider bounds(520, 130, 151, 50) channel("ctrlFunc") range(2, 4, 2, 1, 0.001) text("CtrlFunc") textColour("white")
hslider bounds(520, 130, 151, 50) channel("ctrlAmp") range(.3, 1, .6, 1, 0.001) text("CtrlAmp") textColour("white")

hslider bounds(520, 130, 151, 50) channel("oscFunc") range(2, 4, 3, 1, 0.001) text("OscFunc") textColour("white")

hslider bounds(252, 186, 161, 52) channel("rvbSend") range(0, 1, .25, 1, 0.001) text("Rvb Send") textColour("white")
hslider bounds(426, 188, 150, 50) channel("rvbPan") range(.001, 4, 2, 1, 0.001) text("Rvb Pan") textColour("white")

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
        kTrig metro chnget:k("rate")
    endif

    if changed(kTrig) == 1 then
        event "i", "Trapped07", 0, iDur, iFrq, iAmp 
    endif
   
endin

               instr     Trapped07
                      
ip3             =         chnget:i("dur")
ip4             =         chnget:i("synthLvl")             
ifreq           =         chnget:i("frq")*rnd(chnget:i("rndFrq"))

ip6             =         chnget:i("modFrqStrt")*rnd(.4)
ip7             =         chnget:i("modFrqPeak")*rnd(.6)

kp8            =         chnget:k("ctrlAmp")
ip9            =         int(chnget:i("ctrlFunc"))

ip10           =         int(chnget:i("oscFunc"))
                                                                                                                                                                            
ifuncl         =         512                                                                                                   

kenv           linen     ip4, ip3 * .3, ip3, ip3 * .4   

k1             linseg    ip6, ip3 * .5, ip7, ip3 * .5, ip6       
a3             oscili    kp8, ifreq + k1, ip9            
a4             phasor    ifreq                         
a5             table     (a4 + a3) * ifuncl, ip10 
a5             =         kenv * a5                
a1             oscil     kenv, ifreq + .9, ip10  
                 
kGate         linsegr    0, ip3 * .1, 1, ip3 * .7, 1, ip3*.2, 0 
              
aMix           =         (a1 + a5) * kGate 

               outs      aMix * chnget:k("synthLvl") * chnget:k("masterLvl"), aMix * chnget:k("synthLvl") * chnget:k("masterLvl") 
                          
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