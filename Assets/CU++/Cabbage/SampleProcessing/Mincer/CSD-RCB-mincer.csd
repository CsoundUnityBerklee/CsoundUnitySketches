<CsoundSynthesizer>
<CsOptions>
-n -d 
</CsOptions>
<CsInstruments>

ksmps = 32
nchnls = 2
0dbfs = 1



instr 1

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

idur  = p3
ilock = p4
ipitch = 1
itimescale = 0.15
iamp  = 0.8

atime line   0,idur,idur*itimescale
;asig mincer atimpt, kamp, kpitch, ktab, klock[,ifftsize,idecim]
asig  mincer atime, iamp, ipitch, p5, ilock
      outs asig, asig

endin
</CsInstruments>
<CsScore>

i 1 0 5 0 1	;not locked
i 1 6 5 1 2	;locked
e
i 1 0 5 0 3	;not locked
i 1 6 5 1 4	;locked
s
i 1 0 5 0 5	;not locked
i 1 6 5 1 6	;locked
s
i 1 0 5 0 7	;not locked
i 1 6 5 1 8	;locked
s
i 1 0 5 0 9	;not locked

e
</CsScore>
</CsoundSynthesizer>