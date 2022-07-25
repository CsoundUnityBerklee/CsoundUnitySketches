<Cabbage> bounds(0, 0, 0, 0)
form caption("Trapped09") size(650, 250), guiMode("queue") pluginId("def1")

button  bounds(34, 34, 101, 44) channel("trigger") text("Trigger") textColour("white")

hslider bounds(322, 10, 150, 50) channel("dur") range(2, 9, 4, 1, 0.001) text("Dur") textColour("white")
hslider bounds(250, 68, 150, 50) channel("note") range(20, 80, 50, 1, 0.001) text("MIDI NN") textColour("white")
hslider bounds(408, 68, 150, 50) channel("rndNote") range(0, 8, 3, 1, 0.001) text("Rnd NN") textColour("white")

hslider bounds(484, 10, 150, 50) channel("amp") range(0, 1, .6, 1, 0.001) text("Synth Lvl") textColour("white")

hslider bounds(286, 122, 162, 50) channel("rndRate") range(100, 400, 185, 1, 0.001) text("RndRate") textColour("white")
hslider bounds(452, 122, 150, 50) channel("rndAmp") range(0, 10, 3.3, 1, 0.001) text("RndAmp") textColour("white")

hslider bounds(338, 182, 150, 50) channel("delaySend") range(0, 1, .76, 1, 0.001) text("Delay Send") textColour("white")
hslider bounds(490, 182, 150, 50) channel("delayTime") range(.001, 5, .08, 1, 0.001) text("Delay Time") textColour("white")

hslider bounds(22, 184, 150, 50) channel("rvbSend") range(0, 1, .45, 1, 0.001) text("Rvb Send") textColour("white")
hslider bounds(176, 184, 150, 50) channel("rvbPan") range(0, 6, 4, 1, 0.001) text("Rvb Pan") textColour("white")

hslider bounds(160, 10, 150, 50) channel("masterLvl") range(0, 1, 0.9, 1, 0.001) text("Master Lvl") textColour("white")

combobox bounds(120, 104, 100, 25), populate("*.snaps"), channelType("string") automatable(0) channel("combo31") text("Starting-4note", "More Notes Less Vcomb", "More noise & open filter -pan LFO faster", "faster, less notes, more delay", "Slower, More resonance, and RingMod", "Complex PolyRhythms", "Alll Notes, Spinning, No Delay, All FreeVerb") value("1")
filebutton bounds(58, 104, 60, 25), text("Save", "Save"), populate("*.snaps", "test"), mode("named preset") channel("filebutton32")
filebutton bounds(58, 134, 60, 25), text("Remove", "Remove"), populate("*.snaps", "test"), mode("remove preset") channel("filebutton33")
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

garvb          init      0
gadel          init      0

giSine ftgen 1, 0, 8192, 10, 1
giWav2 ftgen 2, 0, 8192, 10, 10, 8, 0, 6, 0, 4, 0, 1
giWav3 ftgen 3, 0, 8192, 10, 10, 0, 5, 5, 0, 4, 3, 0, 1
giWav4 ftgen 4, 0, 8192, 10, 10, 0, 9, 0, 0, 8, 0, 7, 0, 4, 0, 2, 0, 1

gkRevPan init 4

instr 1
    iDur = chnget:i("dur")
 
    iNote = chnget:i("frq")+rnd(chnget:i("rndNote"))
    iFrq = cpsmidinn(iNote)
    iAmp = chnget:i("amp")
    kTrig chnget "trigger"
    if changed(kTrig) == 1 then
        event "i", "Trapped09", 0, iDur, iFrq, iAmp
    endif
endin


instr     Trapped09

ip3    = chnget:i("dur")

iamp   = chnget:i("amp")*.2
              
kNote  = chnget:k("note")+rnd(chnget:i("rndNote"))
kFrq   = cpsmidinn(kNote)*.25

kRndAmp  = chnget:k("rndAmp")
kRndFrq  = chnget:k("rndRate")
                                                                                        
k2             randh     kRndAmp, kRndFrq, .1                     
k3             randh     kRndAmp * .98, kRndFrq * .91, .2             
k4             randh     kRndAmp * 1.2, kRndFrq * .96, .3            
k5             randh     kRndAmp * .9, kRndFrq * 1.3     
                       
kenv           linen    iamp, ip3 *.2, ip3, ip3 * .8  

a1             oscil     kenv, kFrq + k2, giSine, .2             
a2             oscil     kenv * .91, (kFrq + .004) + k3, giWav2, .3
a3             oscil     kenv * .85, (kFrq + .006) + k4, giWav3, .5
a4             oscil     kenv * .95, (kFrq + .009) + k5, giWav4, .8

kgate         transeg   1, ip3, 0, 0 
amix           =        (a1 + a2 + a3 + a4) * kgate
aL             =        a1 + a3
aR             =        a2 + a4

              outs      aL * chnget:k("masterLvl"), aR * chnget:k("masterLvl")
                
garvb          =         garvb + (amix * chnget:k("rvbSend")) 
gadel          =         gadel + (amix * chnget:k("delaySend")) 
               
endin

               instr     Delay
               denorm    gadel
ip3    = chnget:i("dur")
kgate          expseg    1, ip3*.7, 1, ip3*.3, .0001
asig           delay     gadel, chnget:i("delayTime")
asig = asig*kgate
               outs      asig*chnget:k("masterLvl"), asig*chnget:k("masterLvl")
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
;causes Csound to run for about 7000 years...
f0 z
;starts instrument 1 and runs it for a week
i1 0 [60*60*24*7] 
i "Delay" 0 [60*60*24*7]
i "Reverb" 0 [60*60*24*7]  
</CsScore>
</CsoundSynthesizer>