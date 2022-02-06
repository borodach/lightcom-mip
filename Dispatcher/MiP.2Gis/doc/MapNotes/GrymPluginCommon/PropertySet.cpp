#include "stdafx.h"

#include "PropertySet.h"
#include "Encoding.h"

namespace GPCommon
{

const std::string PropertySet::c_key_separator_(": ");
const std::string PropertySet::c_key_utf8_("utf8");

PropertySet::PropertySet()
{
}

PropertySet::~PropertySet()
{
}

void PropertySet::LoadFromFile(const tstd::tstring &filename)
{
	HANDLE hFile = ::CreateFile(filename.c_str(), GENERIC_READ, FILE_SHARE_READ, 0, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, 0);
	if (INVALID_HANDLE_VALUE == hFile)
	{
		throw tstd::tmsgexception(_T("can't open property set file to read from"));
	}
	try
	{
		std::vector<char> buf(::GetFileSize(hFile, 0));
		if (!buf.empty())
		{
			DWORD readed = 0;
			if (!::ReadFile(hFile, &(buf.front()), DWORD(buf.size()), &readed, 0))
			{
				throw tstd::tmsgexception(_T("can't read property file"));
			}
			if (buf.size() != readed)
			{
				throw tstd::tmsgexception(_T("property file read failed"));
			}
		}
		::CloseHandle(hFile);
		hFile = 0;
		LoadFromMem(std::string(buf.begin(), buf.end()));
	}
	catch(...)
	{
		if (hFile)
		{
			::CloseHandle(hFile);
		}
		throw;
	}
}

void PropertySet::LoadFromMem(const std::string &membuf)
{
	std::stringstream source(membuf, std::ios_base::in/* | std::ios_base::binary*/);
	return LoadFromStream(source);
}

void PropertySet::SaveToFile(const tstd::tstring &filename) const
{
	std::string buf = SaveToMem();
	HANDLE hFile = ::CreateFile(filename.c_str(), GENERIC_WRITE, 0, 0, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, 0);
	if (INVALID_HANDLE_VALUE == hFile)
	{
		DWORD i = ::GetLastError();
		throw tstd::tmsgexception("can't create property set file");
	}
	DWORD written;
	::WriteFile(hFile, buf.c_str(), DWORD(buf.size()), &written, 0);
	::CloseHandle(hFile);
}

std::string PropertySet::SaveToMem() const
{
	std::stringstream destination(std::ios_base::out | std::ios_base::trunc/* | std::ios_base::binary*/);
	SaveToStream(destination);
	return destination.str();
}

tstd::tstring PropertySet::GetValueString(const std::string &key) const /* throw(...) */
{
	property_map_t::const_iterator prop_it = property_map_.find(key);
	if (prop_it != property_map_.end())
	{
		return prop_it->second;
	}
	throw tstd::tmsgexception("value not found");
}

tstd::tstring PropertySet::GetValueStringSafe(const std::string &key, const tstd::tstring &defval) const /* throw() */
{
	property_map_t::const_iterator prop_it = property_map_.find(key);
	return (prop_it != property_map_.end()) ? prop_it->second : defval;
}

long PropertySet::GetValueLong(const std::string &key) const /*throw(...)*/
{
	property_map_t::const_iterator prop_it = property_map_.find(key);
	if (prop_it != property_map_.end())
	{
		return atol(prop_it->second.c_str());
	}
	throw tstd::tmsgexception("value not found");
}

long PropertySet::GetValueLongSafe(const std::string &key, long defval) const /*throw(...)*/
{
	property_map_t::const_iterator prop_it = property_map_.find(key);
	return (prop_it != property_map_.end()) ? atol(prop_it->second.c_str()) : defval;
}

void PropertySet::SetValueString(const std::string &key, const tstd::tstring &val)
{
	property_map_[key] = val;
}

void PropertySet::SetValueLong(const std::string &key, long val)
{
	char buf[20];
	SetValueString(key, itoa(val, buf, 10));
}

void PropertySet::RemoveValue(const std::string &key)
{
	property_map_.erase(key);
}

class PropertySet::SaveFunc
{
public:
	SaveFunc(std::ostream &stream) : stream_(stream) {}
	void operator()(const std::string &key, const tstd::tstring &value)
	{
		stream_
			<< key << c_key_separator_ 
#ifdef _UNICODE
			<< UTF16ToUTF8(tstd::tconvert<wchar_t>(EscapeValue(value))) << std::endl;
#else
			<< tstd::tconvert<char>(EscapeValue(value)) << std::endl;
#endif
	}
private:
	std::ostream &stream_;
};

void PropertySet::SaveToStream(std::ostream &stream) const
{
#ifdef _UNICODE
	stream << c_key_utf8_ << std::endl;
#endif
	ForEachProperty(SaveFunc(stream));
}

tstd::tstring PropertySet::EscapeValue(const tstd::tstring &value)
{
	tstd::tstring result;

	tstd::tstring::const_iterator start = value.begin();
	tstd::tstring::const_iterator val_it = start;
	tstd::tstring::const_iterator val_end = value.end();
	for (;val_it != val_end; ++ val_it)
	{
		switch (*val_it)
		{
			case _T('\n'):
			case _T('\\'):
				if (start != val_it)
				{
					result += tstd::tstring(start, val_it);
				}
				switch (*val_it)
				{
					case _T('\n'):
						result.append(_T("\\n"));
						break;
					case _T('\\'):
						result.append(_T("\\\\"));
						break;
				}
				start = val_it + 1;
				break;
		}
	}
	if (start != val_end)
	{
		result += tstd::tstring(start, val_end);
	}
	return result;
}

tstd::tstring PropertySet::UnescapeValue(const tstd::tstring &value)
{
	tstd::tstring result;

	tstd::tstring::const_iterator start = value.begin();
	tstd::tstring::const_iterator val_it = start;
	tstd::tstring::const_iterator val_end = value.end();
	for (; val_it != val_end; ++val_it)
	{
		if (_T('\\') == *val_it)
		{
			if (start != val_it)
			{
				result += tstd::tstring(start, val_it);
				start = val_it;
			}
			if (++val_it == val_end)
			{
				break;
			}
			switch (*val_it)
			{
				case _T('n'):
					result.append(1, _T('\n'));
					start = val_it + 1;
					break;
				case _T('\\'):
					start = val_it;
					break;
			}
		}
	}
	if (start != val_end)
	{
		result += tstd::tstring(start, val_end);
	}

	return result;
}

void PropertySet::LoadFromStream(std::istream &stream)
{
	property_map_t pm;
	bool first_line = true;
	bool utf8 = false;
	while (stream.good())
	{
		std::stringbuf linebuf;
		if (stream.get(linebuf).eof())
		{
			break;
		}
		std::string line = linebuf.str();
		// skip '\r' character (from \r\n sequence, if any)
		if (!line.empty() && ('\r' == *line.rbegin()))
		{
			line.resize(line.size() - 1);
		}
		if (first_line)
		{
			first_line = false;
			if (line == c_key_utf8_)
			{
				utf8 = true;
				// skip new line character
				std::istream::char_type ch;
				if (stream.get(ch).eof())
				{
					break;
				}
				continue;
			}
		}
		size_t delim = line.find(c_key_separator_);
		if (delim != std::string::npos)
		{
			std::string key(line, 0, delim);
			std::string value_raw(line, delim + c_key_separator_.length());
			tstd::tstring tvalue = utf8
				? tstd::tconvert<tstd::char_type>(UTF8ToUTF16(value_raw))
				: tstd::tconvert<tstd::char_type>(value_raw);
			pm.insert(property_map_t::value_type(key, UnescapeValue(tvalue)));
		}
		// skip new line character
		std::istream::char_type ch;
		if (stream.get(ch).eof())
		{
			break;
		}
	}
	property_map_.swap(pm);
}

} // namespace GPCommon
