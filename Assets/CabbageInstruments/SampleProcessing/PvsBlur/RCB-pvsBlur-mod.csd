<Cabbage> bounds(0, 0, 0, 0)

form caption("Stereo Spectral Blur") size(800, 200), guiMode("queue") pluginId("def1")

button bounds(22, 72, 80, 40) channel("trigger") text("Trigger")

label    bounds(408, 128, 80, 20), text("FFT Size"), fontColour(255, 255, 255, 255) channel("label4")
combobox bounds(410, 154, 76, 24), text("128", "256", "512", "1024", "2048", "4096", "8192"), channel("att_table"), value(3), fontColour(220, 220, 255, 255)

label  bounds(160, 128, 80, 20), text("Sound"), fontColour(255, 255, 255, 255) channel("label44")
combobox bounds(154, 150, 95, 28), channel("sound"), value(3), fontColour(220, 220, 255, 255), text("Drums1", "Brahms", "Cage", "Bus", "Pad", "Drums2")
groupbox bounds(114, 12, 190, 105) channel("groupbox1") text("Blur Time") colour(0, 0, 0, 0) outlineColour(0, 0, 0, 50), textColour(255, 255, 255, 255) fontColour(255, 255, 255, 255) colour(0, 0, 0, 0) fontColour(255, 255, 255, 255) outlineColour(0, 0, 0, 50) textColour(255, 255, 255, 255) colour(0, 0, 0, 0) fontColour(255, 255, 255, 255) outlineColour(0, 0, 0, 50) textColour(255, 255, 255, 255)
rslider bounds(170, 38, 79, 66), channel("blurTime"), range(0, 3, 0.618, 1, 0.01), text("Time"), trackerColour(255, 255, 255, 255), outlineColour(0, 0, 0, 50), textColour(0, 0, 0, 255)

groupbox bounds(308, 12, 190, 105) channel("groupbox2") text("Blur Rate") colour(0, 0, 0, 0) outlineColour(0, 0, 0, 50), textColour(255, 255, 255, 255) fontColour(255, 255, 255, 255) colour(0, 0, 0, 0) fontColour(255, 255, 255, 255) outlineColour(0, 0, 0, 50) textColour(255, 255, 255, 255) colour(0, 0, 0, 0) fontColour(255, 255, 255, 255) outlineColour(0, 0, 0, 50) textColour(255, 255, 255, 255)
rslider bounds(362, 36, 79, 66), channel("blurRate"), range(0, 3, 0.53, 1, 0.01), text("Rate"), trackerColour(255, 255, 255, 255), outlineColour(0, 0, 0, 50), textColour(0, 0, 0, 255)

groupbox bounds(506, 12, 190, 105) channel("groupbox3") text("Blur Depth") colour(0, 0, 0, 0) outlineColour(0, 0, 0, 50), textColour(255, 255, 255, 255) fontColour(255, 255, 255, 255) colour(0, 0, 0, 0) fontColour(255, 255, 255, 255) outlineColour(0, 0, 0, 50) textColour(255, 255, 255, 255) colour(0, 0, 0, 0) fontColour(255, 255, 255, 255) outlineColour(0, 0, 0, 50) textColour(255, 255, 255, 255)
rslider bounds(558, 38, 79, 66), channel("blurDepth"), range(0, 3, 0.35, 1, 0.01), text("Depth"), trackerColour(255, 255, 255, 255), outlineColour(0, 0, 0, 50), textColour(0, 0, 0, 255)

rslider bounds(710, 118, 65, 53), channel("verbLvl"), range(0, 1, 0.65, 1, 0.01), text("Verb"), trackerColour(255, 255, 255, 255), outlineColour(0, 0, 0, 50), textColour(0, 0, 0, 255)
rslider bounds(32, 122, 65, 53), channel("masterLvl"), range(0, 1, 0.618, 1, 0.01), text("Gain"), trackerColour(255, 255, 255, 255), outlineColour(0, 0, 0, 50), textColour(0, 0, 0, 255)
rslider bounds(708, 24, 65, 53), channel("mix"), range(0, 1, 0.25, 1, 0.01), text("Mix"), trackerColour(255, 255, 255, 255), outlineColour(0, 0, 0, 50), textColour(0, 0, 0, 255)
rslider bounds(32, 12, 65, 53), channel("dur"), range(0, 20, 9, 1, 0.01), text("Duration"), trackerColour(255, 255, 255, 255), outlineColour(0, 0, 0, 50), textColour(0, 0, 0, 255)

rslider bounds(272, 128, 65, 53), channel("speed"), range(-0.3, 1.3, 1.1, 1, 0.01), text("Speed"), trackerColour(255, 255, 255, 255), outlineColour(0, 0, 0, 50), textColour(0, 0, 0, 255)

label bounds(344, 128, 53, 18), text("Loop"), fontColour(255, 255, 255, 255)  channel("label66")
checkbox bounds(354, 150, 33, 28), channel("loop"), value(1), fontColour:0(255, 255, 255, 255) colour:1(255, 255, 255, 255)

combobox bounds(558, 126, 100, 25), populate("*.snaps"), channelType("string") automatable(0) channel("combo31")  value("1")
filebutton bounds(496, 126, 60, 25), text("Save", "Save"), populate("*.snaps", "test"), mode("named preset") channel("filebutton32")
filebutton bounds(496, 156, 60, 25), text("Remove", "Remove"), populate("*.snaps", "test"), mode("remove preset") channel("filebutton33")

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
gkDur init 9

maxalloc "Blur", 3

/* FFT attribute tables - from Iain McCurdy*/
giFFTattributes1    ftgen    0, 0, 4, -2,  128,  64,  128, 1
giFFTattributes2    ftgen    0, 0, 4, -2,  256, 128,  256, 1
giFFTattributes3    ftgen    0, 0, 4, -2,  512, 128,  512, 1
giFFTattributes4    ftgen    0, 0, 4, -2, 1024, 256, 1024, 1
giFFTattributes5    ftgen    0, 0, 4, -2, 2048, 512, 2048, 1
giFFTattributes6    ftgen    0, 0, 4, -2, 4096,1024, 4096, 1
giFFTattributes7    ftgen    0, 0, 4, -2, 8192,2048, 8192, 1
;---------------------------------------------------------------------------------------------------------------------------------------
opcode    Reverse, a, aKkkk            ;nb. CAPITAL K CREATE A K-RATE VARIABLE THAT HAS A USEFUL VALUE ALSO AT I-TIME
    ain,ktime,kreverse,kforward,klink    xin            ;READ IN INPUT ARGUMENTS
    ;four windowing envelopes. An appropriate one will be chosen based on the reversed chunk duration
    ienv1    ftgenonce            0, 0, 131072, 7, 0, 1024,           1, 131072-(1024*2),           1,  1024,       0    ;for longest chunk times
    ienv2    ftgenonce            0, 0, 131072, 7, 0, 4096,           1, 131072-(4096*2),           1,  4096,       0
    ienv3    ftgenonce            0, 0, 131072, 7, 0,16384,           1, 131072-(16384*2),          1, 16384,       0
    ienv4    ftgenonce            0, 0, 131072, 7, 0,32768,           1, 131072-(32768*2),          1, 32768,       0    ;for shortest chunk times

    atime    interp    ktime            ;INTERPOLATE TO CREATE A-RATE VERSION OF K-TIME
        
    iratio    =    octave(1)
    
    ktrig    trigger    klink,0.5,0        ;if 'Link L&R' is turned on restart delay time phasors to ensure sync between the two channels
    if ktrig=1 then
     reinit    RESTART_PHASOR
    endif
    RESTART_PHASOR:
    aptr    phasor    (2/ktime)        ;CREATE A MOVING PHASOR THAT WITH BE USED TO TAP THE DELAY BUFFER
    rireturn
    if ktime<0.2 then            ;IF CHUNK TIME IS LESS THAN 0.2... (VERY SHORT) 
     aenv    table3    aptr,ienv4,1        ;CREATE AMPLITUDE ENVELOPE
    elseif ktime<0.4 then
     aenv    table3    aptr,ienv3,1
    elseif ktime<2 then
     aenv    table3    aptr,ienv2,1
    else                    ;other longest bracket of delay times
     aenv    table3    aptr,ienv1,1
    endif
    aptr    =    aptr*atime        ;SCALE PHASOR ACCORDING TO THE LENGTH OF THE DELAY TIME CHOSEN BY THE USER
 
     abuffer    delayr    4 ;+ 0.01        ;CREATE A DELAY BUFFER
    abwd    deltap3    aptr            ;READ AUDIO FROM A TAP WITHIN THE DELAY BUFFER
    afwd    deltap3    atime            ;FORWARD DELAY
        delayw    ain            ;WRITE AUDIO INTO DELAY BUFFER
    
    ;rireturn                ;RETURN FROM REINITIALISATION PASS
    xout    (abwd*aenv*kreverse)+(afwd*kforward)    ;SEND AUDIO BACK TO CALLER INSTRUMENT. APPLY AMPLITUDE ENVELOPE TO PREVENT CLICKS.
endop

;---------------------------------------------------------------------------------------------------------------------------------------

; window functions
giwfn1    ftgen  0,  0, 131072,  9,  0.5, 1,    0                          ; HALF SINE
giwfn2    ftgen  0,  0, 131072,  7,  0, 3072,   1, 128000,     0           ; PERCUSSIVE - STRAIGHT SEGMENTS
giwfn3    ftgen  0,  0, 131072, 16,  0, 3072,0, 1, 128000, -2,  0          ; PERCUSSIVE - EXPONENTIAL SEGMENTS
giwfn4    ftgen  0,  0, 131072,  7,  0, 3536,   1, 124000,     1, 3536, 0  ; GATE - WITH ANTI-CLICK RAMP UP AND RAMP DOWN SEGMENTS
giwfn5    ftgen  0,  0, 131072,  7,  0, 128000, 1, 3072,       0           ; REVERSE PERCUSSIVE - STRAIGHT SEGMENTS
giwfn6    ftgen  0,  0, 131072,  5,  0.001, 128000, 1, 3072,   0.001       ; REVERSE PERCUSSIVE - EXPONENTIAL SEGMENTS
giwfn7    ftgen  0,  0, 131072,  20, 2, 1                                  ; HANNING WINDOW
giwindows ftgen  0,  0, 8, -2, giwfn7, giwfn1, giwfn2, giwfn3, giwfn4

giBufL    ftgen  0, 0, 1048576, -2, 0                                      ; function table used for storing audio
giBufR    ftgen  0, 0, 1048576, -2, 0                                      ; function table used for storing audio

gigaussian ftgen  0, 0, 4096, 20, 6, 1, 1                                  ; a gaussian distribution

gaGMixL, gaGMixR  init  0  ; initialise stereo grain signal


instr 1
  gkDur = chnget:k("dur")
    kTrig chnget "trigger"
    if changed(kTrig) == 1 then
        event "i", "Blur", 0, gkDur
    endif
endin

instr Blur	 ; spectral bluring instrument
 	
kGain      chnget "gain"
kBlurT     chnget "blurTime" 
kLFORate   chnget "blurRate"
kLFODepth  chnget "blurDepth"

/* SET FFT ATTRIBUTES - from Iain McCurdy*/
katt_table  chnget    "att_table"    ; FFT atribute table
katt_table  init    4
ktrig       changed    katt_table
    if ktrig==1 then
      reinit update
    endif
update:
iFFTsize    table    0, giFFTattributes1 + i(katt_table) - 1
ioverlap    table    1, giFFTattributes1 + i(katt_table) - 1
iwinsize    table    2, giFFTattributes1 + i(katt_table) - 1
iwintype    table    3, giFFTattributes1 + i(katt_table) - 1
  

/* INPUT */
kinput chnget "sound"
if kinput=1 then
aInL, aInR	diskin2 "./sounds/drums1.aif", chnget:k("speed"), 0, chnget:i("loop")
elseif kinput=2 then
aInL, aInR	diskin2 "./sounds/brahms.aif", chnget:k("speed"), 0, chnget:i("loop")
elseif kinput=3 then
aInL, aInR	diskin2 "./sounds/cage.aif", chnget:k("speed"), 0, chnget:i("loop")
elseif kinput=4 then
aInL, aInR	diskin2 "./sounds/bus.aif", chnget:k("speed"), 0, chnget:i("loop")
elseif kinput=5 then
aInL, aInR	diskin2 "./sounds/pad.aif", chnget:k("speed"), 0, chnget:i("loop")
else
aInL, aInR	diskin2 "./sounds/drums2.aif", chnget:k("speed"), 0, chnget:i("loop")
endif

aLR = aInL+aInR

kEnv      transeg 1, i(gkDur), 0, 0
kLFO      lfo     kLFODepth,kLFORate,1

fSigLR	    pvsanal  aLR, iFFTsize, ioverlap, iwinsize, iwintype            
fBlur	    pvsblur  fSigLR, kBlurT + kLFO, 1				            	
aOut	    pvsynth  fBlur			         		                    
		         		  
aMix       ntrpol       aOut, aLR, chnget:k("mix") 
           
aMix = aMix * chnget:k("masterLvl") * kEnv
            outs         aMix, aMix

    vincr gaRvb, aMix 

endin

instr Reverb                      
            denorm gaRvb
    aL, aR  reverbsc gaRvb, gaRvb, .8, 10000
            outs  aL*chnget:k("masterLvl")*chnget:k("verbLvl"), aR*chnget:k("masterLvl")*chnget:k("verbLvl")
            clear	gaRvb
endin

instr ReverseInst
ktimeL   chnget "timeL"
ktimeR   chnget "timeR"
kspread  chnget "spread"
kmix     chnget "mix"
klevel   chnget "level"
kreverse chnget "reverse"
kforward chnget "forward"
kTMod    chnget "TMod"
kPMod    chnget "PMod"

/* LINK */
klink chnget "link"            ; if 'Link L&R' is selected
if klink=1&&kTMod=0 then        
 ktrigL    changed    ktimeL,klink
 ktrigR    changed    ktimeR
 if ktrigL=1 then
  chnset    ktimeL,"timeR"
 elseif ktrigR=1 then
  chnset    ktimeR,"timeL"
 endif
endif

a1,a2    ins

if kTMod=1 then                        ; if time modulation is selected....
 if klink=0 then                    ; and if 'link L&R' is off...
  ktime1    rspline    ktimeL,ktimeR,0.2,1        ; generate delay time value: random spline between ktimeL and ktimeR 
  ktime2    rspline    ktimeL,ktimeR,0.2,1
  ktimeL    limit    ktime1,0.01,4            ; assign to delay time variable and limit to prevent out of range values (possible with rspline)
  ktimeR    limit    ktime2,0.01,4
 else
  ktime        rspline    ktimeL,ktimeR,0.2,1        
  ktimeL    limit    ktime,0.01,4
  ktimeR    =    ktimeL                ; right channel delay the same as left
 endif
endif

arev1    Reverse    a1,ktimeL,kreverse,kforward,klink        ; call UDO
arev2    Reverse    a2,ktimeR,kreverse,kforward,klink

if kPMod=1 then                        ; if panning modulation is on...
 kpan    rspline    0,1,0.2,1                ; pan position generated as a random spline
 ap1    =    (arev1*kpan)     + (arev2*(1-kpan))    ; create new left channel
 ap2    =    (arev1*(1-kpan)) + (arev2*kpan)        ; create new right channel
 arev1    =    ap1                    ; reassign left channel to new left channel
 arev2    =    ap2                     ; reassign right channel to new right channel
endif

a1    ntrpol    a1,arev1,kmix            ; dry/wet mix
a2    ntrpol    a2,arev2,kmix
a1    =    a1 * klevel            ; apply level control
a2    =    a2 * klevel
kspread    scale    kspread,1,0.5             ; reScale from range 0 - 1 to 0.5 - 1
aL    sum    a1*kspread,a2*(1-kspread)    ; create stereo mix according to Spread control
aR    sum    a2*kspread,a1*(1-kspread)    ; create stereo mix according to Spread control
    outs    aL,aR
endin

instr  GrainTrig                           ; grain triggering instrument
kGSize1     chnget    "GSize1"     ; grain size limit 1
kGSize2     chnget    "GSize2"     ; grain size limit 2
kDens       chnget    "Dens"       ; grain density
kDly1       chnget    "Dly1"       ; delay time limit 1
kDly2       chnget    "Dly2"       ; delay time limit 2
kTrns1      chnget    "Trns1"      ; transposition in semitones
kTrns2      chnget    "Trns2"
kPanSpread  chnget    "PanSpread"  ; random panning spread
kAmpSpread  chnget    "AmpSpread"  ; random amplitude spread
kFiltSpread chnget    "FiltSpread" ; random filter spread
kreverse    chnget    "reverse"    ; reversal probability
kampdecay   chnget    "ampdecay"   ; amount of delay->amplitude attenuation
kwindow     chnget    "window"     ; window
kDlyDst     chnget    "DlyDst"     ; delay time distribution
kmix        chnget    "mix"        ; dry/wet mix
klevel      chnget    "level"      ; output level (both dry and wet signals)

aL, aR      ins                    ; read audio input
            outs      aL * klevel * (1-kmix), aR * klevel * (1-kmix)

/* WRITE TO BUFFER TABLES */
ilen        =         ftlen(giBufL)     ; table length (in samples)
aptr        phasor    sr/ilen           ; phase pointer used to write to table
aptr        =         aptr * ilen       ; reScale pointer according to table size
            tablew    aL, aptr, giBufL  ; write audio to table
            tablew    aR, aptr, giBufR  ; write audio to table
kptr        downsamp  aptr              ; downsamp pointer to k-rate

ktrig       metro     kDens             ; grain trigger

/* GRAIN SIZE */
kGSize      random    0,1                           ; random value 0 - 1
;kGSize     expcurve  kGSize, 50                    ; exponentially redistribute range 0 - 1
kMinGSize   min       kGSize1, kGSize2              ; find minimum grain size limit
kMaxGSize   max       kGSize1, kGSize2              ; find maximum grain size limit
kGSize      scale     kGSize, kMaxGSize, kMinGSize  ; reScale random value according to minimum and maximum limits

/* DELAY TIME */
kDly        random    0, 1       ; uniform random value 0 - 1
if kDlyDst=1 then                ; if delay time distribution is exponential
 kDly       expcurve  kDly, 100  ; exponential distribution range 0 - 1
elseif kDlyDst=3 then            ; .. or if logarithmic
 kDly       logcurve  kDly, 100  ; exponential distribution range 0 - 1
endif                            ; (other linear so do not alter)
if kDly1=kDly2 then
 kMinDly    =         kDly1         ; delays can't be the same value!!   
 kMaxDly    =         kDly2 + 0.001
else  
 kMinDly    min       kDly1, kDly2  ; find minimum delay time limit
 kMaxDly    max       kDly1, kDly2  ; find maximum delay time limit
endif
ioffset     =         sr                      ; delay offset (can't read at same location as write pointer!)
ioffset     =         1/ioffset
kDly        scale     kDly, kMaxDly, kMinDly  ; distribution reScaled to match the user defined limits

/* CALL GRAIN */
;                     p1 p2         p3     p4   p5         p6       p7   p8              p9             p10       p11         p12     p13        p14         p15    p16
schedkwhen  ktrig,0,0,2,kDly+0.0001,kGSize,kptr,kPanSpread,kreverse,kDly,kMinDly+ioffset,kMaxDly+0.0001,kampdecay,klevel*kmix,kwindow,kAmpSpread,kFiltSpread,kTrns1,kTrns2  ; call grain instrument
  endin

instr  GrainInst        ; grain instrument
iGStart     =      p4                      ; grain start position (in samples)
ispread     =      p5                      ; random panning spread
ireverse    =      (rnd(1) > p6 ? 1 : -1)  ; decide fwd/bwd status
iwindow     table  p12 - 1, giwindows         ; amplitude envelope shape for this grain

/* AMPLITUDE CONTROL */
idly         =     p7          ; delay time
iMinDly      =     p8          ; minimum delay
iMaxDly      =     p9          ; maximum delay
iampdecay    =     p10          ; amount of delaytime->amplitude attenuation
ilevel       =     p11          ; grain output level
iAmpSpread   =     p13
iFiltSpread  =     p14

iRto         divz    idly - iMinDly , iMaxDly - iMinDly, 0  ; create delay:amplitude ration (safely)
iamp         =       (1 - iRto) ^ 2                         ; invert range
iamp         ntrpol  1, iamp, iampdecay                     ; mix flat amplitude to scaled amplitude according to user setting
iRndAmp      random  1 - iAmpSpread, 1                      ; random amplitude value for this grain
iamp         =       iamp * iRndAmp                         ; apply random amplitude

/* TRANSPOSITION */
iTrns        random  p15, p16
iRto         =       semitone(iTrns)
if iRto>1 then
 iStrtOS     =       (iRto-1)  * sr * p3
else
 iStrtOS     =       0
endif

aline        line    iGStart - iStrtOS, p3, iGStart - iStrtOS + (p3 * iRto * sr * ireverse)  ; grain pointer
aenv         oscili  iamp, 1/p3, iwindow                                                     ; amplitude envelope
aL           tablei  aline, giBufL, 0, 0, 1                                                  ; read audio from table 
aR           tablei  aline, giBufR, 0, 0, 1                                                  ; read audio from table

if iFiltSpread>0 then
 iRndCfOct   random  14 - (iFiltSpread * 10), 14
 iRndCf      =       cpsoct(iRndCfOct)
 aL          butlp   aL, iRndCf
 aR          butlp   aR, iRndCf
endif

ipan         random  0.5 - (ispread * 0.5), 0.5 + (ispread * 0.5)  ; random pan position for this grain
gaGMixL      =       gaGMixL + (aL * aenv * ipan*ilevel)           ; left channel mix added to global variable
gaGMixR      =       gaGMixR + (aR * aenv * (1 - ipan) * ilevel)   ; right channel mix added to global variable
endin

instr  GMix            ; output instrument (always on)
             outs    gaGMixL, gaGMixR                              ; send global audio signals to output
             clear   gaGMixL, gaGMixR                              ; clear global audio variables
endin
</CsInstruments>
<CsScore>
;causes Csound to run for about 7000 years...
f0 z
;starts instrument 1 and runs it for a week
i1 0 [60*60*24*7] 
i "Reverb" 0 [60*60*24*7] 
i "ReverseInst" 0 [60*60*24*7] 
i "GrainTrig" 0 [60*60*24*7] 
i "GMix" 0 [60*60*24*7] 
</CsScore>

</CsoundSynthesizer>
<bsbPanel>
 <label>Widgets</label>
 <objectName/>
 <x>100</x>
 <y>100</y>
 <width>320</width>
 <height>240</height>
 <visible>true</visible>
 <uuid/>
 <bgcolor mode="nobackground">
  <r>255</r>
  <g>255</g>
  <b>255</b>
 </bgcolor>
</bsbPanel>
<bsbPresets>
</bsbPresets>
