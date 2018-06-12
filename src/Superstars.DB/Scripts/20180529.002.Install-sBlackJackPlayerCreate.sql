create proc sp.sBlackJackPlayerCreate
(
	@UserId int,
	@BlackJackPlayerId int out,
	@PlayerCards nvarchar(25),
	@NbTurn int,
	@HandValue int
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

    insert into sp.tBlackJackPlayer(BlackJackPlayerId,BlackJackGameId,PlayerCards,NbTurn,HandValue) values(@UserId, @BlackJackGameId, @PlayerCards, @NbTurn,@HandValue);
	set @BlackJackPlayerId = @UserId;
	commit;
    return 0;
end;