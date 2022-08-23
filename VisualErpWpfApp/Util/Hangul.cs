using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AquilaErpWpfApp3
{    
    public class Hangul
    {
        private const int KIYEOK = 0x1100;  //ㄱ
        private const int A = 0x1161;       //ㅏ
        private const int GA = 0xac00;      //가

        private const int CHO_COUNT = 0x0013;   //유니코드 안의 조합형에서 가능한 초성 글자 수
        private const int JUNG_COUNT = 0x0015;  //유니코드 안의 조합형에서 가능한 중성 글자 수
        private const int JONG_COUNT = 0x001c;  //유니코드 안의 조합형에서 가능한 종성 글자 수

        private const char JUNG_INIT_CHAR = '.';
        private const char JONG_INIT_CHAR = '.';

        private string _currentChar;
        private string _result;
        private State _state;        

        private int _cho;
        private int _jung;
        private char _jungFirst;
        private bool _jungPossible;
        private int _jong;
        private char _jongFirst;
        private char _jongLast;
        private bool _jongPossible;

        public Hangul()
        {
            _currentChar = string.Empty;
            _result = string.Empty;
            _state = State.Cho;

            _cho = -1;
            _jung = -1;
            _jungFirst = JUNG_INIT_CHAR;    //중성 저장 - 두 개의 모음이나 완성되지 않은 글자안에서 순서대로 삭제시 필요
            _jong = -1;
            _jongFirst = JONG_INIT_CHAR;    //첫 번째 종성 저장 - 
            _jongLast = JONG_INIT_CHAR;
        }

        private char GetSingleJa(int value) //하나의 자음으로만 구성된 완성형 글자를 반환
        {
            byte[] bytes = BitConverter.GetBytes((short)(0x1100 + value));
            return Char.Parse(Encoding.Unicode.GetString(bytes));
        }


        private char GetSingleMo(int value) //하나의 모음으로만 구성된 완성형 글자를 반환
        {
            byte[] bytes = BitConverter.GetBytes((short)(0x1161 + value));
            return Char.Parse(Encoding.Unicode.GetString(bytes));
        }

        private char GetCompleteChar()
        {
            int tempJong = 0;
            if (_jong < 0)
                tempJong = 0;
            else
                tempJong = _jong;
            //초성 중성 종성순의 모든 경우를 나열한 유니코드의 한글 코드표안에서 완성형 글을 찾는 공식
            //초성의 수에 중성과 종성의 모든 경우의 수를 곱하면 같은 중성과 종성을 가지고 초성만 바뀌는 글자를
            //찾을 수 있다.
            //초성을 찾은 후에 원하는 중성의 순번에 종성의 모든 경우의 수를 곱하면 순번에 맞는 중성을
            //찾을 수 있다.
            //종성이 조합의 가장 마지막이므로 원하는 종성의 순번만큼을 더하면 원하는 종성을
            //찾을 수 있다.
            int completeChar = (_cho * (JUNG_COUNT * JONG_COUNT)) + (_jung * JONG_COUNT) + tempJong + GA;  //완성형 글자 코드
            byte[] naeBytes = BitConverter.GetBytes((short)(completeChar)); //바이트 배열로 전환
            return Char.Parse(Encoding.Unicode.GetString(naeBytes));    //바이트 배열을 유니코드로 전환.
        }

        private char Filter(char ch)    //Shift 없는 키들은 대문자가 들어와도 소문자로 변환
        {
            if (ch == 'A') ch = 'a';
            if (ch == 'B') ch = 'b';
            if (ch == 'C') ch = 'c';
            if (ch == 'D') ch = 'd';
            //E
            if (ch == 'F') ch = 'f';
            if (ch == 'G') ch = 'g';
            if (ch == 'H') ch = 'h';
            if (ch == 'I') ch = 'i';
            if (ch == 'J') ch = 'j';
            if (ch == 'K') ch = 'k';
            if (ch == 'L') ch = 'l';
            if (ch == 'M') ch = 'm';
            if (ch == 'N') ch = 'n';
            //O
            //P
            //Q
            //R
            if (ch == 'S') ch = 's';
            //T
            if (ch == 'U') ch = 'u';
            if (ch == 'V') ch = 'v';
            //W
            if (ch == 'X') ch = 'x';
            if (ch == 'Y') ch = 'y';
            if (ch == 'Z') ch = 'z';

            return ch;
        }

        public void Input(ref string source, char ch)
        {
            ch = Filter(ch);

            int code = (int)ch;
            if (code == 8)
            {
                if (source.Length <= 0)
                    return;

                if (_state == State.Cho)    //완성상태
                {
                    source = source.Substring(0, source.Length - 1);
                }
                else if (_state == State.Jung && _jungFirst.Equals(JUNG_INIT_CHAR))
                {   //자음까지 진행 상태
                    _state = State.Cho;
                    source = source.Substring(0, source.Length - 1);
                }
                else if (_jungPossible &&   //모음 중 첫 번째가 8,13,18중 하나이고
                    (_jung != 8 && _jung != 13 && _jung != 18)  //현재 모음이 8, 13, 18이 아닐 경우. 즉, 8,13,18중 하나를 거쳐 두 개의 모음인 경우
                    && _jongFirst.Equals(JONG_INIT_CHAR) && _jongLast.Equals(JONG_INIT_CHAR))
                {   //모음 두 개까지 진행 상태
                    _state = State.Jung;
                    source = source.Substring(0, source.Length - 1);
                    _jung = CheckJung(_jungFirst.ToString());
                    _jong = -1;
                    source += GetCompleteChar();
                    _jungPossible = true;
                }
                else if ((_state == State.Jong || _state == State.Jung) && !_jungFirst.Equals(JUNG_INIT_CHAR)
                    && _jongFirst.Equals(JONG_INIT_CHAR) && _jongLast.Equals(JONG_INIT_CHAR))
                {   //모음 하나까지 진행 상태
                    _state = State.Jung;
                    _jungFirst = JUNG_INIT_CHAR;
                    _jungPossible = false;
                    _jung = -1;
                    source = source.Substring(0, source.Length - 1);
                    source += GetSingleJa(_cho);
                }
                else if (_state == State.Jong && !_jongFirst.Equals(JONG_INIT_CHAR) && !_jongLast.Equals(JONG_INIT_CHAR))
                {   //받침 두 개까지 진행 상태
                    _state = State.Jong;
                    source = source.Substring(0, source.Length - 1);
                    _jongLast = JONG_INIT_CHAR;
                    _jong = CheckJong(_jongFirst.ToString());
                    source += GetCompleteChar();
                    _jongPossible = true;
                }
                else if (_state == State.Jong && !_jongFirst.Equals(JONG_INIT_CHAR))
                {   //받침 하나까지 진행 상태
                    //두 개 이상의 모음으로 진행 가능할 경우
                    int temp = CheckJung(_jungFirst.ToString());
                    if (temp == 8 || temp == 13 || temp == 18)
                    {
                        _jungPossible = true;
                        _state = State.Jung;
                    }
                    else
                    {
                        _state = State.Jong;
                    }
                    source = source.Substring(0, source.Length - 1);
                    _jong = -1;
                    _jongFirst = JONG_INIT_CHAR;
                    source += GetCompleteChar();
                }
                return;
            }

            if (!((code >= 97 && code <= 122) || (code >= 65 && code <= 90)))
            {
                _cho = -1;
                _jung = -1;
                _jong = -1;
                _jungFirst = JUNG_INIT_CHAR;
                _jongFirst = JONG_INIT_CHAR;
                _jongLast = JONG_INIT_CHAR;
                _state = State.Cho;
                source += ch;
                return;
            }

            if (_state == State.Cho)
            {
                _cho = CheckCho(ch);

                if (_cho >= 0)
                {
                    _state = State.Jung;
                    source += GetSingleJa(_cho);
                }
                else //모음이 먼저 입력됐을 때
                {
                    _state = State.Jung;
                    Input(ref source, ch);
                }
            }
            else if (_state == State.Jung)
            {

                if (_jung < 0)
                {
                    _jung = CheckJung(ch.ToString());
                    if (_jung < 0)    //자음이 입력됐을 때
                    {
                        _state = State.Cho;
                        Input(ref source, ch);
                        return;
                    }

                    if (_cho < 0)    //모음이 먼저 입력됐을 때
                    {
                        //source = source.Substring(0, source.Length-1);
                        source += GetSingleMo(CheckJung(ch.ToString()));
                        _state = State.Cho; //초기화
                        _jung = -1;         //초기화
                        return;
                    }
                    else
                    {
                        //두 개 이상의 모음으로 진행 가능할 경우
                        if (_jung == 8 || _jung == 13 || _jung == 18)
                        {
                            _jungPossible = true;
                            _state = State.Jung;
                        }
                        else
                        {
                            _state = State.Jong;
                        }
                        _jungFirst = ch;
                        source = source.Substring(0, source.Length-1);
                        source += GetCompleteChar();
                    }
                }
                else //두 개 이상의 모음으로 진행 가능한 경우였을 경우.
                {
                    string jung = string.Empty;
                    jung += _jungFirst;
                    jung += ch;

                    int temp = CheckJung(jung);
                    if (temp > 0)   //두 개 이상의 모음일 경우
                    {
                        _jung = temp;
                        source = source.Substring(0, source.Length-1);
                        source += GetCompleteChar();
                        _state = State.Jong;
                    }
                    else //그 외가 들어왔을 경우 '종'으로 넘긴다.
                    {
                        _state = State.Jong;
                        Input(ref source, ch);
                    }

                    //_jungFirst = JUNG_INIT_CHAR;
                }
            }
            else if (_state == State.Jong)
            {
                if (_jong < 0)
                {
                    _jong = CheckJong(ch.ToString());

                    if (_jong > 0)
                    {
                        source = source.Substring(0, source.Length - 1);
                        source += GetCompleteChar();

                        _jongFirst = ch;
                        if (_jong == 1 || _jong == 4 || _jong == 8 || _jong == 17)
                        //두 개 이상의 자음으로 받침이 구성될 수 있는 경우
                        {
                            _jongPossible = true;
                        }
                    }
                    else if(CheckJung(ch.ToString()) >= 0)//모음이 들어왔을 경우
                    {
                        _state = State.Jung;
                        _cho = -1;
                        _jung = -1;
                        Input(ref source, ch);
                        return;
                    }
                    else if (CheckCho(ch) >= 0)//받침으로 쓸 수 없는 자음이 들어왔을 경우
                    {
                        _jongPossible = false;
                        _jong = 0;
                        Input(ref source, ch);
                    }
                }
                else //두 개 이상의 자음으로 받침이 구성될 수 있었던 경우 
                     //또는 받침이 완성되어 다음 문자가 초성이 되어 새로운 문자를 생성하거나
                     //아니면 중성이 되어 받침을 초성으로 가져갈 경우
                {
                    if (_jongPossible)
                        //두 개 이상의 자음으로 받침이 구성될 수 있는 경우
                    {
                        _jongPossible = false;
                        string jong = string.Empty;
                        jong += _jongFirst;
                        jong += ch;

                        int temp = CheckJong(jong);

                        if (temp > 0)   //두 개 이상의 자음으로 받침이 구성될 경우
                        {
                            _jongLast = ch;
                            _jong = temp;
                            source = source.Substring(0, source.Length - 1);
                            source += GetCompleteChar();
                        }
                        else //그 외가 들어왔을 경우 다시 '종'으로 넘긴다.
                        {
                            //_state == State.Jong;

                            Input(ref source, ch);
                        }
                    }
                    else //받침이 완성이 되었을 경우
                    {
                        if (CheckCho(ch) >= 0)   //자음이 들어왔을 경우
                        {
                            _jongFirst = JONG_INIT_CHAR;
                            _jongLast = JONG_INIT_CHAR;

                            //'초'로 보낸다.
                            _state = State.Cho;
                            _jung = -1;
                            _jong = -1;
                            _jungFirst = JUNG_INIT_CHAR;
                            _jungPossible = false;
                            Input(ref source, ch);
                        }
                        else //모음이 들어왔을 경우
                        {
                            if (_jongLast.Equals(JONG_INIT_CHAR))   //받침이 하나인 경우
                            {
                                //받침 없는 전 글자를 다시 채우고
                                source = source.Substring(0, source.Length - 1);
                                _jong = 0;
                                source += GetCompleteChar();

                                //받침을 초성으로 둔 후
                                _cho = CheckCho(_jongFirst);
                            }
                            else //받침이 두 개인 경우.
                            {
                                //받침 하나인 전 글자를 다시 채우고
                                source = source.Substring(0, source.Length - 1);
                                _jong = CheckJong(_jongFirst.ToString());
                                source += GetCompleteChar();

                                //두 번째 받침을 초성으로 둔 후
                                _cho = CheckCho(_jongLast);
                            }
                            source += GetSingleJa(_cho);

                            // '중'으로 보낸다.
                            _jongFirst = JONG_INIT_CHAR;
                            _jongLast = JONG_INIT_CHAR;
                            _jungPossible = false;
                            _jung = -1;
                            _jong = -1;
                            _state = State.Jung;
                            Input(ref source, ch);
                        }
                    }
                }
            }
        }
        
        private int CheckCho(char ch)
        {
            foreach (Cho c in Enum.GetValues(typeof(Cho)))
            {
                if (c.ToString().Equals(ch.ToString()))
                {
                    return (int)c;
                }
            }
            return -1;
        }

        private int CheckJung(string ch)
        {
            foreach (Jung j in Enum.GetValues(typeof(Jung)))
            {
                if (ch.Equals(j.ToString()))
                {
                    return (int)j;
                }
            }
            return -1;
        }

        private int CheckJong(string ch)
        {
            foreach (Jong j in Enum.GetValues(typeof(Jong)))
            {
                if (ch.Equals(j.ToString()))
                {
                    return (int)j;
                }
            }
            return -1;
        }

        public void InitState()
        {
            _cho = -1;
            _jung = -1;
            _jong = -1;
            _state = State.Cho;
            _jungPossible = false;
            _jongPossible = false;
            _jungFirst = JUNG_INIT_CHAR;
            _jongFirst = JONG_INIT_CHAR;
            _jongLast = JONG_INIT_CHAR;
        }
    }    
}