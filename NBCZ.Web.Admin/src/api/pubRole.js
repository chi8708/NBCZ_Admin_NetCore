import axios from '@/libs/api.request'
import store from '@/store'

var token=store.state.user.token;
export const getRoles = () => {
  return axios.request({
    url: 'api/PubRole/GetList',
    headers: {Authorization:"Bearer "+token},
    method: 'post'
  })
}