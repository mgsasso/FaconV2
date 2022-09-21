insert into Facon.dbo.TblDDTClienti
([CodiceFornitore]
	  ,[ArticoloCliente]
      ,[DescrizioneArticoloCliente]
      ,[CommessaCliente]
      ,[DataBollaCliente]
      ,[NumeroBollaCliente]
      ,[CausaleLavorazione]
      ,[QT]
      ,[PrezzoUnitario]
      ,[Note]
	  )
select tas400.* from
(
select * from openquery(as400sql, 'select distinct JVCFOR,  JVCST1||JVCST2||''/''||JVCMOD||''/''||JVCART AS ARTICOLOCLIENTE, Rtrim(T1XDES) || '' '' || Rtrim(L1XDES) AS DESCRIZIONE, CASE J2NRAG WHEN 0 THEN JVNRAG||''/''||JVNCOM ELSE J2NRAG||''/''||J2NCOM END AS COMMESSACLIENTE,
JVDBOL AS DATABOLLACLIENTE, JVNBOL||''/''||JVSIGL AS NUMEROBOLLACLIENTE, 1 AS CAUSALELAVORAZIONE, JVQTA1 AS QT, 
CASE WHEN JVNRAG >= 9000 THEN CASE JVCOPE WHEN 4 THEN ROUND((J2PEUR/2) * 1.4,2) ELSE ROUND(J2PEUR * 1.4,2) END ELSE CASE WHEN JVQTA1 <= 100 THEN CASE JVCOPE WHEN 4 THEN ROUND((J2PEUR/2) * 1.2,2) ELSE ROUND(J2PEUR * 1.2,2) END ELSE CASE JVCOPE WHEN 4 THEN ROUND(J2PEUR/2,2) ELSE J2PEUR END END END AS PREZZOUNITARIO,
case JVctop when 22 then case JVcope when 1 then ''CONFEZIONE POST TRATTAMENTO/TINTURA'' when 2 then ''CONFEZIONE IMBASTITURA'' when 3 then ''CONFEZIONE INVISIBILE'' when 4 then ''CONFEZIONE PARZIALE'' else '''' end end as note 
from applibmgs.terst00f 
LEFT JOIN APPLIBMGS.ANAPF00F on JVcst1 = g1cst1 and JVcst2 = g1cst2 and JVcart = g1cart and JVcmod = g1cmod
LEFT JOIN APPLIBMGS.ANCAM00F
ON G1CST1 = L1CST1 AND G1CST2 = L1CST2 AND G1CART = L1CART AND G1CMOD = L1CMOD
LEFT JOIN (SELECT * FROM APPLIBMGS.TBORD00F WHERE T1TREC = ''CLS'') TBORD ON digits(G1CCLS) = SUBSTR(T1KEYT, 9, 2) AND G1CMAR = SUBSTR(T1KEYT, 7, 2)
LEFT JOIN APPLIBMGS.ANCMP00F ON 
		L1CCMP = T3CCMP
LEFT JOIN APPLIBMGS.LOPES00F ON J2CTOP = JVCTOP AND J2COPE = JVCOPE AND J2CST1 = JVCST1 AND J2CST2 = JVCST2 AND J2CART = JVCART AND J2CMOD = JVCMOD and J2CFOR = JVCFOR
WHERE JVCST1 >= Year(CurDate()) - 1 and JVfann = '''' and j2fann = '''' and jvdbol >= 20220801
order by JVdbol asc
')) tas400
left join Facon.dbo.TblDDTClienti t0
on tas400.NUMEROBOLLACLIENTE = t0.NumeroBollaCliente and tas400.DATABOLLACLIENTE = t0.DataBollaCliente and tas400.ARTICOLOCLIENTE = t0.ArticoloCliente and tas400.COMMESSACLIENTE = t0.CommessaCliente
where t0.DataBollaCliente is null