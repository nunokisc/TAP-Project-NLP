
/** TF E DF **/
select * from words where id = 1

(SELECT offs.offset, offs.docid FROM words as wd
	inner join relation as rl ON rl.wordid = wd.id
		inner join docs as dc ON rl.docid = dc.id
			inner join offsetword as offs ON offs.wordid = wd.id AND offs.docid = dc.id
			where  word = 'Salvador' )
UNION ALL
(SELECT offs.offset, offs.docid FROM words as wd
	inner join relation as rl ON rl.wordid = wd.id
		inner join docs as dc ON rl.docid = dc.id
			inner join offsetword as offs ON offs.wordid = wd.id AND offs.docid = dc.id
			where  word = 'sobral')

SELECT * FROM (SELECT offs.offset, offs.docid FROM words as wd
	inner join relation as rl ON rl.wordid = wd.id
		inner join docs as dc ON rl.docid = dc.id
			inner join offsetword as offs ON offs.wordid = wd.id AND offs.docid = dc.id
			where  word = 'Salvador' ) as sl1 
				inner join docs as dc ON sl1.docid = dc.id	
					inner join (SELECT offs.offset, offs.docid FROM words as wd
						inner join relation as rl ON rl.wordid = wd.id
							inner join docs as dc ON rl.docid = dc.id
								inner join offsetword as offs ON offs.wordid = wd.id AND offs.docid = dc.id
								where  word = 'sobral') as sl2 
									ON sl1.docid = sl2.docid AND sl2.offset = sl1.offset+1
									

SELECT word, offs.offset, offs.docid FROM words as wd
	inner join relation as rl ON rl.wordid = wd.id
		inner join docs as dc ON rl.docid = dc.id
			inner join offsetword as offs ON offs.wordid = wd.id AND offs.docid = dc.id
				where word = 'salvador' 

SELECT offs.offset, offs.docid FROM words as wd
	inner join relation as rl ON rl.wordid = wd.id
		inner join docs as dc ON rl.docid = dc.id
			inner join offsetword as offs ON offs.wordid = wd.id AND offs.docid = dc.id
				where word = 'sobral' 


SELECT * FROM relation WHERE wordid = 2

SELECT * FROM offsetword WHERE wordid = 1



select * from relation
select * from words where word='kiev'

SELECT * FROM (SELECT offs.offset, offs.docid FROM words as wd inner join relation as rl ON rl.wordid = wd.id inner join docs as dc ON rl.docid = dc.id inner join offsetword as offs ON offs.wordid = wd.id AND offs.docid = dc.id where  word = 'Salvador' ) as sl1 inner join docs as dc ON sl1.docid = dc.id	 inner join (SELECT offs.offset, offs.docid FROM words as wd inner join relation as rl ON rl.wordid = wd.id inner join docs as dc ON rl.docid = dc.id inner join offsetword as offs ON offs.wordid = wd.id AND offs.docid = dc.id where word = 'Sobral') as sl2 ON sl1.docid = sl2.docid AND sl2.offset = sl1.offset +1 inner join (SELECT offs.offset, offs.docid FROM words as wd inner join relation as rl ON rl.wordid = wd.id inner join docs as dc ON rl.docid = dc.id inner join offsetword as offs ON offs.wordid = wd.id AND offs.docid = dc.id where word = 'Kiev') as sl3 ON sl1.docid = sl3.docid AND sl3.offset = sl1.offset +3 inner join (SELECT offs.offset, offs.docid FROM words as wd inner join relation as rl ON rl.wordid = wd.id inner join docs as dc ON rl.docid = dc.id inner join offsetword as offs ON offs.wordid = wd.id AND offs.docid = dc.id where word = 'festivais') as sl4 ON sl1.docid = sl4.docid AND sl4.offset = sl1.offset +6