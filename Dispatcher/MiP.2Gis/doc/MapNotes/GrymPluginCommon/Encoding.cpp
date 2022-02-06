#include "StdAfx.h"
#include ".\encoding.h"

namespace GPCommon
{

// original: http://dev.w3.org/cvsweb/XML/encoding.c
/*
 * encoding.c : implements the encoding conversion functions needed for XML
 *
 * Related specs: 
 * rfc2044        (UTF-8 and UTF-16) F. Yergeau Alis Technologies
 * rfc2781        UTF-16, an encoding of ISO 10646, P. Hoffman, F. Yergeau
 * [ISO-10646]    UTF-8 and UTF-16 in Annexes
 * [ISO-8859-1]   ISO Latin-1 characters codes.
 * [UNICODE]      The Unicode Consortium, "The Unicode Standard --
 *                Worldwide Character Encoding -- Version 1.0", Addison-
 *                Wesley, Volume 1, 1991, Volume 2, 1992.  UTF-8 is
 *                described in Unicode Technical Report #4.
 * [US-ASCII]     Coded Character Set--7-bit American Standard Code for
 *                Information Interchange, ANSI X3.4-1986.
 *
 * Original code for IsoLatin1 and UTF-16 by "Martin J. Duerst" <duerst@w3.org>
 *
 * See Copyright for the status of this software.
 *
 * Daniel.Veillard@w3.org
 */

/*
* From rfc2044: encoding of the Unicode values on UTF-8:
*
* UCS-4 range (hex.)           UTF-8 octet sequence (binary)
* 0000 0000-0000 007F   0xxxxxxx
* 0000 0080-0000 07FF   110xxxxx 10xxxxxx
* 0000 0800-0000 FFFF   1110xxxx 10xxxxxx 10xxxxxx 
*
* I hope we won't use values > 0xFFFF anytime soon !
*/

bool UTF16ToUTF8(const std::wstring &in, std::vector<char> &out)
{
  for (std::wstring::const_iterator it_in = in.begin(), it_end = in.end(); it_in != it_end; ++it_in)
  {
    unsigned long c = *it_in;
    if ((c & 0xFC00) == 0xD800)
    { /* surrogates */
      if (it_end == ++it_in)
      {
        return false;
      }
      unsigned long d = *it_in;
      if ((d & 0xFC00) == 0xDC00)
      {
        c &= 0x03FF;
        c <<= 10;
        c |= d & 0x03FF;
        c += 0x10000;
      }
      else
      {
        return false;
      }
    }
    /* assertion: c is a single UTF-4 value */
    int bits;
    if (c < 0x80) {out.push_back(static_cast<char>(c)); bits = -6;}
    else if (c < 0x800) {out.push_back(static_cast<char>(((c >>  6) & 0x1F) | 0xC0)); bits = 0;}
    else if (c < 0x10000) {out.push_back(static_cast<char>(((c >> 12) & 0x0F) | 0xE0)); bits = 6;}
    else {out.push_back(static_cast<char>(((c >> 18) & 0x07) | 0xF0)); bits = 12;}
    for(; bits >= 0; bits -=6)
    {
      out.push_back(static_cast<char>(((c >> bits) & 0x3F) | 0x80));
    }
  }
  return true;
}

std::string UTF16ToUTF8(const std::wstring &in)
{
  std::vector<char> buf;
  if (UTF16ToUTF8(in, buf))
  {
    return std::string(buf.begin(), buf.end());
  }
  return std::string();
}

bool UTF8ToUTF16(const std::string &in, std::vector<wchar_t> &out)
{
  for (std::string::const_iterator it_in = in.begin(), it_end = in.end(); it_in != it_end; ++it_in)
  {
    unsigned char d = *it_in;
    unsigned long c;
    int trailing;
    if (!(d & 0x80)) {c = d; trailing = 0;}
    else if (!(d & 0x40)) {/* trailing byte in leading position */ return false;}
    else if (!(d & 0x20)) {c = d & 0x1F; trailing = 1;}
    else if (!(d & 0x10)) {c = d & 0x0F; trailing = 2;}
    else if (!(d & 0x08)) {c = d & 0x07; trailing = 3;}
    else {/* no chance for this in UTF-16 */ return false;}
    while (trailing--)
    {
      if ((it_end == ++it_in) || (((d = *it_in) & 0xC0) != 0x80)) {return false;}
      c <<= 6;
      c |= d & 0x3F;
    }
    /* assertion: c is a single UTF-4 value */
    if (c < 0x10000)
    {
      out.push_back(static_cast<wchar_t>(c));
    }
    else if (c < 0x110000)
    {
      c -= 0x10000;
      out.push_back(static_cast<wchar_t>(0xD800 | (c >> 10)));
      out.push_back(static_cast<wchar_t>(0xDC00 | (c & 0x03FF)));
    }
    else
    {
      return false;
    }
  }
  return true;
}

std::wstring UTF8ToUTF16(const std::string &in)
{
  std::vector<wchar_t> buf;
  if (UTF8ToUTF16(in, buf))
  {
    return std::wstring(buf.begin(), buf.end());
  }
  return std::wstring();
}

} // namespace GPCommon
