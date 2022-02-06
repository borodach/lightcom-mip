#pragma once

#include <map>

#include "tstd.h"

namespace GPCommon
{

class PropertySet
{
public:
	PropertySet();
	~PropertySet();

	void LoadFromFile(const tstd::tstring &filename);
	void LoadFromMem(const std::string &membuf);
	void SaveToFile(const tstd::tstring &filename) const;
	std::string SaveToMem() const;

	tstd::tstring GetValueString(const std::string &key) const /* throw(...) */;
	tstd::tstring GetValueStringSafe(const std::string &key, const tstd::tstring &defval) const /* throw() */;
	long GetValueLong(const std::string &key) const /*throw(...)*/;
	long GetValueLongSafe(const std::string &key, long defval) const /*throw(...)*/;

	void SetValueString(const std::string &key, const tstd::tstring &val);
	void SetValueLong(const std::string &key, long val);

	void RemoveValue(const std::string &key);

	// void Func(const std::string &key, tstd::tstring &value)
	template <class Func>
	Func ForEachProperty(Func f)
	{
		for (property_map_t::iterator it = property_map_.begin(), end = property_map_.end(); it != end; ++it)
		{
			f(it->first, it->second);
		}
		return f;
	}
	// void Func(const std::string &key, const tstd::tstring &value)
	template <class Func>
	Func ForEachProperty(Func f) const
	{
		for (property_map_t::const_iterator it = property_map_.begin(), end = property_map_.end(); it != end; ++it)
		{
			f(it->first, it->second);
		}
		return f;
	}
private:
	class SaveFunc;
	
	typedef std::map<std::string, tstd::tstring> property_map_t;
	property_map_t property_map_;

	void SaveToStream(std::ostream &stream) const;
	void LoadFromStream(std::istream &stream);

	static tstd::tstring EscapeValue(const tstd::tstring &value);
	static tstd::tstring UnescapeValue(const tstd::tstring &value);

	static const std::string c_key_separator_;
	static const std::string c_key_utf8_;
};

} // namespace GPCommon
