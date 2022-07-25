<Cabbage> bounds(0, 0, 0, 0)

form caption("Sample Mincer") size(600, 200), guiMode("queue") pluginId("def1")

button bounds(26, 74, 80, 40) channel("trigger") text("Trigger")

label  bounds(136, 124, 80, 20), text("Sound"), fontColour(255, 255, 255, 255) channel("label44")
combobox bounds(130, 146, 95, 28), channel("sound"), value(1), fontColour(220, 220, 255, 255), text("Whisper", "Dog", "Traffic", "Bird", "QBfox", "DrB-hello", "Perotin", "Sheila", "Drum")

groupbox bounds(118, 14, 190, 105) channel("groupbox1") text("Time") colour(0, 0, 0, 0) outlineColour(0, 0, 0, 50), textColour(255, 255, 255, 255) fontColour(255, 255, 255, 255) colour(0, 0, 0, 0) fontColour(255, 255, 255, 255) outlineColour(0, 0, 0, 50) textColour(255, 255, 255, 255) colour(0, 0, 0, 0) fontColour(255, 255, 255, 255) outlineColour(0, 0, 0, 50) textColour(255, 255, 255, 255)
rslider bounds(174, 40, 79, 66), channel("time"), range(0, 10, 2, 1, 0.01), text("Time"), trackerColour(255, 255, 255, 255), outlineColour(0, 0, 0, 50), textColour(0, 0, 0, 255)

groupbox bounds(312, 14, 190, 105) channel("groupbox2") text("Pitch") colour(0, 0, 0, 0) outlineColour(0, 0, 0, 50), textColour(255, 255, 255, 255) fontColour(255, 255, 255, 255) colour(0, 0, 0, 0) fontColour(255, 255, 255, 255) outlineColour(0, 0, 0, 50) textColour(255, 255, 255, 255) colour(0, 0, 0, 0) fontColour(255, 255, 255, 255) outlineColour(0, 0, 0, 50) textColour(255, 255, 255, 255)
rslider bounds(366, 38, 79, 66), channel("pitch"), range(-2, 2, .8, 1, 0.01), text("Pitch"), trackerColour(255, 255, 255, 255), outlineColour(0, 0, 0, 50), textColour(0, 0, 0, 255)

rslider bounds(514, 96, 65, 51), channel("verbLvl"), range(0, 1, 0.75, 1, 0.01), text("Verb"), trackerColour(255, 255, 255, 255), outlineColour(0, 0, 0, 50), textColour(0, 0, 0, 255)
rslider bounds(34, 122, 65, 53), channel("masterLvl"), range(0, 1, 0.818, 1, 0.01), text("Gain"), trackerColour(255, 255, 255, 255), outlineColour(0, 0, 0, 50), textColour(0, 0, 0, 255)

rslider bounds(512, 34, 65, 53), channel("mix"), range(0, 1, 0.25, 1, 0.01), text("Mix"), trackerColour(255, 255, 255, 255), outlineColour(0, 0, 0, 50), textColour(0, 0, 0, 255)

rslider bounds(36, 14, 65, 53), channel("dur"), range(.2, 30, 10, 1, 0.01), text("Duration"), trackerColour(255, 255, 255, 255), outlineColour(0, 0, 0, 50), textColour(0, 0, 0, 255)

label bounds(234, 122, 53, 18), text("Loop"), fontColour(255, 255, 255, 255)  channel("label66")
checkbox bounds(244, 144, 33, 28), channel("loop"), value(1), fontColour:0(255, 255, 255, 255) colour:1(255, 255, 255, 255)

combobox bounds(392, 122, 100, 25), populate("*.snaps"), channelType("string") automatable(0) channel("combo31")  value("1") text("DronyBlur", "explosive", "swishy", "groovy", "dreamy", "sleepy")
filebutton bounds(330, 122, 60, 25), text("Save", "Save"), populate("*.snaps", "test"), mode("named preset") channel("filebutton32")
filebutton bounds(330, 152, 60, 25), text("Remove", "Remove"), populate("*.snaps", "test"), mode("remove preset") channel("filebutton33")

</Cabbage>
<CsoundSynthesizer>
<CsOptions>
-n -d
</CsOptions>
<CsInstruments>

ksmps = 32
nchnls = 2
0dbfs = 1

gaRvb init 0

instr 1
  kDur = chnget:k("dur")
    kTrig chnget "trigger"
    if changed(kTrig) == 1 then
        event "i", "Mincer", 0, kDur
    endif
endin

instr Mincer	 ; spectral bluring instrument

Sfile1 = "./sounds/am_whisperM.aif"
Sfile2 = "./sounds/am_dogM.aif"
Sfile3 = "./sounds/am_trafficM.aif"
Sfile4 = "./sounds/am_blackbirdM.aif"
Sfile5 = "./sounds/sp_foxM.wav"
Sfile6 = "./sounds/sp_hellorcbBerkleeM.aif"
Sfile7 = "./sounds/mu_PerotinM.wav"
Sfile8 = "./sounds/mu_SheilaM.wav"
Sfile9 = "./sounds/dl_breakM.aif"

giSmp1  ftgen 1, 0, 0, 1, Sfile1, 0, 0, 0
giSmp2  ftgen 2, 0, 0, 1, Sfile2, 0, 0, 0
giSmp3  ftgen 3, 0, 0, 1, Sfile3, 0, 0, 0
giSmp4  ftgen 4, 0, 0, 1, Sfile4, 0, 0, 0
giSmp5  ftgen 5, 0, 0, 1, Sfile5, 0, 0, 0
giSmp6  ftgen 6, 0, 0, 1, Sfile6, 0, 0, 0
giSmp7  ftgen 7, 0, 0, 1, Sfile7, 0, 0, 0
giSmp8  ftgen 8, 0, 0, 1, Sfile8, 0, 0, 0
giSmp9  ftgen 9, 0, 0, 1, Sfile9, 0, 0, 0
 
iDur       = chnget:i("dur")	
kGain      = chnget:k("masterlevel")
kTime      = chnget:k("time") 
kPitch     = chnget:k("pitch")

/* INPUT */
kinput chnget "sound"

if kinput=1 then
iLen1  filelen Sfile1 
aSig   diskin2 Sfile1, chnget:k("pitch"), 0, chnget:i("loop")
aTime  line   0,iLen1,iLen1*i(kTime)
;asig mincer atimpt, kamp, kpitch, ktab, klock[,ifftsize,idecim]
aMinc  mincer aTime, 1, kPitch, 1, 0 ;ilock

elseif kinput=2 then
iLen2  filelen Sfile2 
aSig   diskin2 Sfile2, chnget:k("pitch"), 0, chnget:i("loop")
aTime  line   0,iLen2,iLen2*i(kTime)
aMinc  mincer aTime, 1, kPitch, 2, 0 ;ilock

elseif kinput=3 then
iLen3  filelen Sfile3
aSig   diskin2 Sfile3, chnget:k("pitch"), 0, chnget:i("loop")
aTime  line   0,iLen3,iLen3*i(kTime)
aMinc  mincer aTime, 1, kPitch, 3, 0 ;ilock

elseif kinput=4 then
iLen4  filelen Sfile4
aSig   diskin2 Sfile4, chnget:k("pitch"), 0, chnget:i("loop")
aTime  line   0,iLen4,iLen4*i(kTime)
aMinc  mincer aTime, 1, kPitch, 4, 0 ;ilock

elseif kinput=5 then
iLen5  filelen Sfile5
aSig   diskin2 Sfile5, chnget:k("pitch"), 0, chnget:i("loop")
aTime  line   0,iLen5,iLen5*i(kTime)
aMinc  mincer aTime, 1, kPitch, 5, 0 ;ilock

elseif kinput=6 then
iLen6  filelen Sfile6
aSig   diskin2 Sfile6, chnget:k("pitch"), 0, chnget:i("loop")
aTime  line   0,iLen6,iLen6*i(kTime)
aMinc  mincer aTime, 1, kPitch, 6, 0 ;ilock

elseif kinput=7 then
iLen7  filelen Sfile7
aSig   diskin2 Sfile7, chnget:k("pitch"), 0, chnget:i("loop")
aTime  line   0,iLen7,iLen7*i(kTime)
aMinc  mincer aTime, 1, kPitch, 7, 0 ;ilock

elseif kinput=8 then
iLen8  filelen Sfile8
aSig   diskin2 Sfile8, chnget:k("pitch"), 0, chnget:i("loop")
aTime  line   0,iLen8,iLen8*i(kTime)
aMinc  mincer aTime, 1, kPitch, 8, 0 ;ilock

else
iLen9  filelen Sfile9
aSig   diskin2 Sfile9, chnget:k("pitch"), 0, chnget:i("loop")
aTime  line   0,iLen9,iLen8*i(kTime)
aMinc  mincer aTime, 1, kPitch, 9, 0 ;ilock

endif

aOut       ntrpol       aSig, aMinc, chnget:k("mix") 
           
aOut = aOut * chnget:k("masterLvl"); * kEnv
            outs         aOut, aOut

    vincr gaRvb, aOut 

endin

instr Reverb                      
            denorm gaRvb
    aL, aR  reverbsc gaRvb, gaRvb, .8, 10000
            outs  aL*chnget:k("masterLvl")*chnget:k("verbLvl"), aR*chnget:k("masterLvl")*chnget:k("verbLvl")
            clear	gaRvb
endin

</CsInstruments>
<CsScore>
f0 z
i1 0 [60*60*24*7] 
i "Reverb" 0 [60*60*24*7]  
</CsScore>
</CsoundSynthesizer>