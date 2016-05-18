#- coding: utf-8 -*-.
import urllib2
import re
import pinyin
import codecs

response = urllib2.urlopen(u"http://quote.eastmoney.com/stocklist.html")
content = response.read()
content = content.strip()
pattern = re.compile(r'<li><a target="_blank" href="http://quote.eastmoney.com/(?P<code>\w*).html">(?P<name>.*)\(.*\)</a></li>')

result = u""
pinyin = pinyin.PinYin()
for m in re.finditer(pattern, content):
	code = m.groups()[0][2:].decode(u'gb2312')
	name = m.groups()[1]
	if not isinstance(name, unicode):
		try:
			name = name.decode(u'gb2312')
		except:
			name = name.decode(u'gbk')

	if u'退市' in name:
		continue;
		
	main_type = u'未知'
	submain_type1 = u''
	submain_type2 = u''
	abbr = pinyin.hanzi2pinyin_split(string=name,split=u"", firstcode=True)
	abbr = abbr.replace(u'*', u'')
	if u'重庆' in name:
		abbr = abbr.replace(u'zq', u'cq')

	if not isinstance(abbr, str):
		abbr = abbr

	if code[0] == u'0':
		main_type = u"深圳A股"
		if code[2] == u'2':
			submain_type1 = u'中小板'
	if code[0] == u'3':
		main_type = u'深圳A股'
		submain_type1 = u'创业板'
	
	if code[0] == u'6':
		main_type = u'上海A股'
	
	if code[0] == u'2':
		main_type = u'深圳B股'

	if code[0] == u'9':
		main_type = u'上海B股'

	if code[0] == u'1':
		if code[1] == u'1' or code[1] == u'3':
			main_type = u'深圳其它'
		if code[1] == u'8' or code[1] == u'5' or code[1] == u'6':
			main_type = u'深圳基金'

	if code[0]== u'5':
		main_type = u'上海基金'
	
	result += code + u" " + name + u" " + main_type + u" " + submain_type1 + u" " + submain_type2 + u" " + abbr
	result.encode('utf-8')
	result += "\r\n"
	
file = codecs.open(u'd:\\stockcode0.txt', 'w', encoding='UTF-8')
file.write(result)
file.close()
