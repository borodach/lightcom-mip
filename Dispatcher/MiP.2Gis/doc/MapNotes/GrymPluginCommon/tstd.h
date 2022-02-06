#pragma once

#include <string>
#include <sstream>
#include <exception>

namespace tstd
{
#ifdef _UNICODE
typedef wchar_t char_type;
#else
typedef char char_type;
#endif
typedef std::basic_string<char_type> tstring;

typedef std::basic_stringbuf<char_type> tstringbuf;
typedef std::basic_istringstream<char_type> tistringstream;
typedef std::basic_ostringstream<char_type> tostringstream;
typedef std::basic_stringstream<char_type> tstringstream;

template <typename destination_char_type>
std::basic_string<destination_char_type> tconvert(const std::basic_string<char> &source);

template <>
inline std::basic_string<char> tconvert<char>(const std::basic_string<char> &source)
{
	return source;
}
template <>
std::basic_string<wchar_t> tconvert<wchar_t>(const std::basic_string<char> &source);

template <typename destination_char_type>
std::basic_string<destination_char_type> tconvert(const std::basic_string<wchar_t> &source);

template <>
inline std::basic_string<wchar_t> tconvert<wchar_t>(const std::basic_string<wchar_t> &source)
{
	return source;
}
template <>
std::basic_string<char> tconvert<char>(const std::basic_string<wchar_t> &source);

// the base class for exceptions
// the derived exceptions are compatible with standard std::exception semantics
class texception : public std::exception
{
public:
	texception() {}
	~texception() {}
	virtual const char_type *twhat() const /*throw()*/ = 0;
	virtual const char *what() const
	{
#ifdef _UNICODE
		msg_tmp_ = tconvert<char>(twhat());
		return msg_tmp_.c_str();
#else
		return twhat();
#endif
	}
#ifdef _UNICODE
private:
	mutable std::string msg_tmp_;
#endif
};

class tmsgexception : public texception
{
public:
	explicit tmsgexception(const tstring &msg) : msg_(msg) {}
	virtual ~tmsgexception() {}
	virtual const char_type *twhat() const /*throw()*/ {return msg_.c_str();}
private:
	tstring msg_;
};

} // namespace tstd

#ifndef _T
# ifdef _UNICODE
#   define _T(x) L ## x
# else
#   define _T(x) x
# endif
#endif
