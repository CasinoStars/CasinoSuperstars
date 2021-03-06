import { postAsync, getAsync, getAsyncNoJSON, postAxios, postSync } from "../helpers/apiHelper";
const endpoint = "/api/wallet";

class WalletApiService {
    constructor() {
    }

    async AddCoins(model) {
        return await postAsync(`${endpoint}/AddCoins`, model);
    }

    async CreditPlayerInBTC(pot) {
        return await postAsync(`${endpoint}/${pot}/creditBTCPlayer`);
    }

    async CreditPlayerInFake(pot) {
        return await postAsync(`${endpoint}/${pot}/creditFakePlayer`);
    }

    async WithdrawFakeBankRoll(pot) {
        return await postAsync(`${endpoint}/${pot}/withdrawFakeBank`);
    }

    async WithdrawBTCBankRoll(pot) {
        return await postAsync(`${endpoint}/${pot}/withdrawBTCBank`);
    }

    async GetFakeBalance() {
        return await getAsync(`${endpoint}/FakeBalance`);
    }

    async GetTrueBalance() {
        return await getAsync(`${endpoint}/TrueBalance`);
    }

    async GetWalletAddress(){
        return  await getAsyncNoJSON(`${endpoint}/GetAddress`);
    }

    async Withdraw(model){
        var response = await postAsync(`${endpoint}/Withdraw`, model);
        return response.text();
    }

    async isPseudoExist(model) {
        var response = await postAsync(`${endpoint}/isPseudoExist`, model);
        return response.text();

    }


    async GetTransaction(maxConfirmation){
        var response = await getAsync(`${endpoint}/${maxConfirmation}/GetTransaction`);
        return response;
    }

    async GetTransfer(){
        var response = await getAsync(`${endpoint}/GetTransfer`);
        return response;
    }

    async FindById(userId){
        var response = await getAsync(`${endpoint}/${userid}/FindById`);
        return response;
    }
    

    async Transfer(model){
        await postAsync(`${endpoint}/TransferToPlayer`, model);
    }

    async GetBTCBankRoll(){
        return await getAsync(`${endpoint}/BTCBankRoll`);   
    }

    async GetFakeBankRoll(){
        return await getAsync(`${endpoint}/FakeBankRoll`);   
    }

     async IsValidAddress(model){
        return await postAsync(`${endpoint}/AddressValidator`,model);   
    }
}

export default new WalletApiService();