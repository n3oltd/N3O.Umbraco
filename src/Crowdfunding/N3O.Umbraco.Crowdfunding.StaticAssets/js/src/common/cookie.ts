export const getCrowdfundingCookie = () => {
    const cookieName = 'Crowdfunding-API-Key';

    const existingCookie = document.cookie.split('; ').find(row => row.startsWith(`${cookieName}=`));
    return  existingCookie?.split('=')[1];
}