using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AquilaErpWpfApp3
{
    public enum State
    {
        Cho, //Begin a new character
        Jung, //Processed by one or two Moum.
        Jong    //Processed by Jong-Seong with one or two Jaum.
    }

    public enum Cho
    {
        None = -1,
        r = 0,  //ㄱ
        R = 1,  //ㄲ
        s = 2,  //ㄴ
        e = 3,  //ㄷ
        E = 4,  //ㄸ
        f = 5,  //ㄹ
        a = 6,  //ㅁ
        q = 7,  //ㅂ
        Q = 8,  //ㅃ
        t = 9,  //ㅅ
        T = 10, //ㅆ
        d = 11, //ㅇ
        w = 12, //ㅈ
        W = 13, //ㅉ
        c = 14, //ㅊ
        z = 15, //ㅋ
        x = 16, //ㅌ
        v = 17, //ㅍ
        g = 18  //ㅎ
    }

    public enum Jung
    {
        None = -1,
        k = 0,  //ㅏ
        o = 1,  //ㅐ
        i = 2,  //ㅑ
        O = 3,  //ㅒ
        j = 4,  //ㅓ
        p = 5,  //ㅔ
        u = 6,  //ㅕ
        P = 7,  //ㅖ
        h = 8,  //ㅗ
        hk = 9, //ㅘ
        ho = 10,//ㅙ
        hl = 11,//ㅚ
        y = 12, //ㅛ
        n = 13, //ㅜ
        nj = 14,//ㅝ
        np = 15,//ㅞ
        nl = 16,//ㅟ
        b = 17, //ㅠ
        m = 18, //ㅡ
        ml = 19,//ㅢ
        l = 20 //ㅣ
    }
    
    public enum Jong
    {
        None = -1,
        r   = 1,    //ㄱ
        R   = 2,    //ㄲ
        rt  = 3,    //ㄳ
        s   = 4,    //ㄴ
        sw  = 5,    //ㄵ
        sg  = 6,    //ㄶ
        e   = 7,    //ㄷ
        f   = 8,    //ㄹ
        fr  = 9,    //ㄺ
        fa  = 10,   //ㄻ
        fq  = 11,   //ㄼ
        ft  = 12,   //ㄽ
        fx  = 13,   //ㄾ
        fv  = 14,   //ㄿ
        fg  = 15,   //ㅀ
        a   = 16,   //ㅁ
        q   = 17,   //ㅂ
        qt  = 18,   //ㅄ
        t   = 19,   //ㅅ
        T   = 20,   //ㅆ
        d   = 21,    //ㅇ
        w   = 22,   //ㅈ
        c   = 23,   //ㅊ
        z   = 24,   //ㅋ
        x   = 25,   //ㅌ
        v   = 26,   //ㅍ
        g   = 27    //ㅎ
    }
}