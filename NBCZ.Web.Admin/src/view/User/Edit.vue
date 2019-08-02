<template>
  <div>
    <!-- Id, UserCode, UserName, RealName, UserPwd, Sex, IdentityNo, Birthday, DeptCode, ManagerFlag, Tel, EMail, QQ, Remark, StopFlag, Crid, Crdt, Lmid, Lmdt, LoginDate, ProvinceCode, CityCode, RegionCode, UserAddress, Wxcode, HeadUrl -->
    <Form ref="formInline" label-position="right" :model="Row" :rules="ruleUser" :label-width="100">
      <Row>
        <Col span="12">
          <FormItem label="用户名" prop="userName">
            <Input v-model="Row.userName" />
          </FormItem>
        </Col>
        <Col span="12">
          <FormItem label="密码" prop="userPwd">
            <Input type="password" v-model="Row.userPwd" />
          </FormItem>
        </Col>
      </Row>
      <Row>
        <Col span="12">
          <FormItem label="电话">
            <Input v-model="Row.tel" />
          </FormItem>
        </Col>
        <Col span="12">
          <FormItem label="部门">
            <Input search enter-button v-model="Row.deptName" readonly @on-search="deptSelect" />
            <!-- <Tree :data="DeptTree"></Tree> -->
            <!-- <Input type="password" v-model="Row.deptCode" /> -->
          </FormItem>
        </Col>
      </Row>
      <Row>
        <Col span="12">
          <FormItem label="姓名">
            <Input v-model="Row.realName" />
          </FormItem>
        </Col>
        <Col span="12">
          <FormItem label="性别">
            <RadioGroup v-model="Row.sex">
              <Radio :label="1">
                <span>男</span>
              </Radio>
              <Radio :label="0">
                <span>女</span>
              </Radio>
            </RadioGroup>
          </FormItem>
        </Col>
      </Row>
      <Row>
        <Col span="24">
          <FormItem label="角色">
            <Select v-model="Row.roleCodes" filterable multiple>
              <Option
                v-for="item in Roles"
                :value="item.roleCode"
                :key="item.roleCode"
              >{{ item.roleName }}</Option>
            </Select>
          </FormItem>
        </Col>
      </Row>
      <Row>
        <Col span="24">
          <FormItem label="地址">
            <Input v-model="Row.userAddress" />
          </FormItem>
        </Col>
      </Row>
      <Row>
        <Col span="24">
          <FormItem label="备注">
            <Input v-model="Row.remark" />
          </FormItem>
        </Col>
      </Row>
      <Row>
        <Col span="24">
          <div style="text-align:center;">
            <Button @click="parent.modelEdit=false">取消</Button>
            <Button style="margin-left:20px;" type="primary" @click="save('formInline')">保存</Button>
          </div>
        </Col>
      </Row>
    </Form>

    <Modal
      title="选择部门"
      :mask-closable="false"
      v-model="modelDept"
      width="300"
      scrollable
      footer-hide
    >
      <Tree :data="DeptTree" @on-select-change="deptTreeChange"></Tree>
    </Modal>
  </div>
</template>
<script>
import { getRoles } from "@/api/pubRole";
import { getDepts } from "@/api/pubDept";
import { add as addUser, edit as editUser } from "@/api/pubUser";
import { truncate, truncateSync } from "fs";
export default {
  props: { editRow: Object, parent: Object },
  computed: {},
  data() {
    return {
      Row: {},
      Roles: [],
      Depts: [],
      DeptTree: [],
      DeptTreeItems: [],
      modelDept: false,
      ruleUser: {
        userName: [
          {
            required: true,
            message: "用户名必填",
            trigger: "blur"
          }
        ],
        userPwd: [
          {
            required: true,
            message: "密码必填",
            trigger: "blur"
          },
          {
            type: "string",
            min: 6,
            message: "密码长度≥6",
            trigger: "blur"
          }
        ]
      }
    };
  },
  methods: {
    // saveEdit() {
    //   //console.log(this.Row);
    //   //this.parent.modalEdit=true;
    // },
    getRoles() {
      getRoles()
        .then(res => {
          const resData = res.data;
          const data = resData.data;
          this.Roles = data;
        })
        .catch(err => {});
    },
    getDepts() {
      getDepts()
        .then(res => {
          const resData = res.data;
          const data = resData.data;
          this.Depts = data;
          this.loadDeptTree();
        })
        .catch(err => {});
    },
    loadDeptTree() {
      var treeRoot = this.Depts.filter(p => p.parentCode == "");
      this.loadDeptTreeChild(treeRoot);
    },
    loadDeptTreeChild(treeData) {
      for (let index = 0; index < treeData.length; index++) {
        const element = treeData[index];
        this.loadDeptTreeItem(element);
        const child = this.Depts.filter(p => p.parentCode == element.deptCode);
        if (child && child.length > 0) {
          this.loadDeptTreeChild(child);
        }
      }
    },
    loadDeptTreeItem(treeItemData) {
      var treeItem = {
        title: treeItemData.deptName,
        expand: true,
        value: treeItemData.deptCode
      };
      this.DeptTreeItems.push(treeItem);
      if (treeItemData.parentCode) {
        //foreach 无法终止循环
        this.DeptTreeItems.every(element => {
          if (element.value == treeItemData.parentCode) {
            if (!element.children) {
              element.children = [];
            }
            element.children.push(treeItem);
            return false;
          }
          return true;
        });
      } else {
        this.DeptTree.push(treeItem);
      }
    },
    deptSelect() {
      if (!this.Depts || this.Depts.length <= 0) {
        this.getDepts();
      }

      this.modelDept = true;
    },
    deptTreeChange(data) {
      var item0 = data[0];
      this.Row.deptName = item0.title;
      this.Row.deptCode = item0.value;
      this.modelDept = false;
    },
    save() {
      if (this.parent.isAdd) {
        this.saveAdd();
      } else {
        this.saveEdit();
      }
    },
    saveAdd() {
      this.saveValidate().then(r => {
        if (r) {
          addUser(this.Row)
            .then(res => {
              const resData = res.data;
              const data = resData.data;
              const code = resData.code;
              const msg = resData.msg;
              if (code == 1) {
                this.$Message.info("添加成功");
                this.parent.modelEdit=false;
                this.parent.setPageData(1);
              } else {
                this.$Message.error({content: msg,duration: 10,closable: true});
              }
            })
            .catch(err => {});
        }
      });
    },
    saveEdit() {
      this.saveValidate().then(r => {
        if (!r) {
          return;
        }
      });
      editUser(this.Row)
        .then(res => {
          const resData = res.data;
          const data = resData.data;
          const code = resData.code;
          const msg = resData.msg;
          if (code == 1) {
            this.$Message.info("编辑成功");
            this.parent.modelEdit=false;
            this.parent.setPageData();
          } else {
            this.$Message.error({ content: msg, duration: 10, closable: true });
          }
        })
        .catch(err => {});
    },
    saveValidate(name = "formInline") {
      return this.$refs[name].validate(valid => {
        if (!valid) {
          this.$Message.warning("请检查表单数据！");
          return false;
        } else {
          return true;
        }
      });
    },
    handleReset(name = "formInline") {
      this.$refs[name].resetFields();
    }
  },
  watch: {
    editRow(newVal, oldVal) {
      this.Row = Object.assign({}, newVal);
      this.Row.sex = this.Row.sex === false ? 0 : 1;
    }
  },
  mounted() {
    this.getRoles();
  }
};
</script>

