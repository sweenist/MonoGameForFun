# The problem of fonts
_07-20-2020_

I wanted an 8-bit looking font to go with the pixel art look of my game.
Here are the paths I went down:

### DaFont
I've used it before. Though the 8-bit pixel looking fonts I found were "free" the license lent itself
to anything but. The licensing was a mix of vague and threatening at the same time. Screw that!

### Making my own font!
This was a bit of its own misadventure. Sort of like how dying in a video game is
a teachable moment, I felt like I was dying a lot. I created a raster graphic in gimp of my fonts
(heavily adapted from the original LOZ font).

### FontForge and InkScape
I downloaded FontForge. Documentation/internet/Menu items
were pointing toward importing vector graphics. No problem. I'll download Inkscape!

More sadness. When I tried converting my image to a path, everything got smoothed out. I changed
settings from one extreme to another and everything was curvy. Not at all what I wanted.
More frustration. I tried making "pixels" out of rectangles but everything was floating point and awful.
I didn't want to do this exercise for a couple dozen glyphs. No thank you!

I read up a bit more and someone solved this problem (http://monsterfacegames.blogspot.com/2013/10/pixel-art-tutorial-making-logo-for-your.html). I got a better feel for FontForge's capabilities.
You can even make bitmap outlines. But there's a catch. You also have to download another tool for
autotracing. Then the blog goes on to say that he used a bash script to call the autotracing tool
with arguments that avoided the curviness that the autotracing tries to do. Doesn't work on my
Windows machine.

### FontForge... on LINUX!
I got upstairs and install FontForge and Potrace on my linux box.
I ssh'd my FontForge files to the Linux machine and lo... the version of the file
is too advanced and unreadable. The linux executable for FontForge is a year older
than the Windows build. Sigh...

### The Last Place I looked: An Online PixelFont TTF maker
I did another internet search "fontforge making pixel font". Watched a video on "Make a Pixel Art".
(https://youtu.be/VwrNcxCGnUY). I always feel gross about online tools... maybe because I'm used
to the days of needing to convert files and the site was bloated with ads and click bait. Much like
going to SourceForge these days... And because the base url seemed a bit sketchy, too. yal dot cc...

Well, https://yal.cc/r/20/pixelfont/ seems pretty legit. I uploaded my raster graphic and a couple minutes
later I had a TTF file! No way! Thanks Vadim (that's the guy who runs the website. Kind of a super
genius of sorts!)

##Conclusion:
For now, I sing the praises of the online bitmap to TTF converter. It's a shame I couldn't get
over the hump of learning vector graphics things a bit quicker. I'm pretty happy with how
easy it is to make a font given the right tools... Internet wins again!