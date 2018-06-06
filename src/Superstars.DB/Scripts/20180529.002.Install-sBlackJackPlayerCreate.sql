create proc sp.sBlackJackPlayerCreate
(
	@BlackJackPlayerId int out,
	@PlayerCards nvarchar(25)
)
as

	declare @BlackJackGameId int;
	set @BlackJackGameId = (select top 1 GameId from sp.tGames order by StartDate desc);

begin
	set transaction isolation level serializable;
	begin tran;

	if exists(select * from sp.tBlackJackPlayer bj where bj.BlackJackGameId = @BlackJackGameId)
	begin
		rollback;
		return 1;
	end;

    insert into sp.tBlackJackPlayer(BlackJackPlayerId,BlackJackGameId,PlayerCards) values(@BlackJackPlayerId, @BlackJackGameId, @PlayerCards);
	set @BlackJackPlayerId = scope_identity();
	commit;
    return 0;
end;