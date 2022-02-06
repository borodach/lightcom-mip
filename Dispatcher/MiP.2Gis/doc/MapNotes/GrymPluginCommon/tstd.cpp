#include "stdafx.h"

#include "tstd.h"

template <>std::basic_string<wchar_t> tstd::tconvert<wchar_t>(const std::basic_string<char> &source)
{
	if (!source.empty())
	{
		if (int wide_buf_len = ::MultiByteToWideChar(CP_ACP, 0, source.c_str(), -1, 0, 0))
		{
			std::vector<wchar_t> buf(wide_buf_len);
			UNI_VERIFY(wide_buf_len == ::MultiByteToWideChar(CP_ACP, 0, source.c_str(), -1, &buf.front(), wide_buf_len));
			return std::basic_string<wchar_t>(&buf.front(), wide_buf_len - 1); // the last character is \0
		}
	}
	return std::basic_string<wchar_t>();
}

template <>std::basic_string<char> tstd::tconvert<char>(const std::basic_string<wchar_t> &source)
{
	if (!source.empty())
	{
		if (int mb_buf_len = ::WideCharToMultiByte(CP_ACP, 0, source.c_str(), -1, 0, 0, 0, 0))
		{
			std::vector<char> buf(mb_buf_len);
			UNI_VERIFY(mb_buf_len == ::WideCharToMultiByte(CP_ACP, 0, source.c_str(), -1, &buf.front(), mb_buf_len, 0, 0));
			return std::basic_string<char>(&buf.front(), mb_buf_len - 1); // the last character is \0
		}
	}
	return std::basic_string<char>();
}
