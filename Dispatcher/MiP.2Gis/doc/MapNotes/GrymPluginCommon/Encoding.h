#pragma once

#include <string>
#include <vector>

namespace GPCommon
{

bool UTF16ToUTF8(const std::wstring &in, std::vector<char> &out);
std::string UTF16ToUTF8(const std::wstring &in);
bool UTF8ToUTF16(const std::string &in, std::vector<wchar_t> &out);
std::wstring UTF8ToUTF16(const std::string &in);

} // namespace GPCommon
