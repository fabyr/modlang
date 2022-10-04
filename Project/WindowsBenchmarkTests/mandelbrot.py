maxIter = 100

def Mandel(x, y):
    zre = 0
    zim = 0
    zretmp = 0
    i = 0
    while i < maxIter:
        zretmp = zre
        zre = (zre * zre) - (zim * zim)
        zim = 2 * zretmp * zim
        
        zre += x
        zim += y
        
        if zre * zre + zim * zim > 4:
            break
        
        i += 1
    
    if i == maxIter:
        return -1
    return i

def Map(x, in_min, in_max, out_min, out_max):
    return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min
    

def MANDEL():
    x = -2.1
    y = 1
    w = 2.6
    h = -2
    xm = 0
    ym = 0
    cw = 70
    ch = 20
    
    cy = 0
    while cy < ch:
        cx = 0
        while cx < cw:
            xm = Map(cx, 0, cw, x, x + w)
            ym = Map(cy, 0, ch, y, y + h)
            
            r = Mandel(xm ,ym)
            if(r == (-1)):
                print("$", end='')
            else:
                print("'", end='')
            
            cx += 1
        print()
        cy += 1
        
if __name__ == '__main__':
    import timeit
    elapsed = timeit.timeit("MANDEL()", number=500, globals=locals())
    print(f"{elapsed * 1000}ms")