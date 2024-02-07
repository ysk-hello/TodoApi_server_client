<template>
    <h1 class="mb-5">Todo List</h1>

    <!--formで囲むと、リダイレクトしてしまう-->
    <!--ログを保持に設定-->

    <div id="addForm" class="mb-5" v-if="isShowAddForm">
        <h2>Add</h2>
        <div class="row">
            <div class="col-auto">
                <input type="text" class="form-control" placeholder="new todo" v-model="addForm.name" />
            </div>
            <div class="col-auto">
                <button class="btn btn-primary" @click="addItem()">Add</button>
            </div>
        </div>
    </div>

    <div id="editForm" class="mb-5" v-if="isShowEditForm">
        <h2>Edit</h2>
        <input type="hidden" id="edit-id" v-model="editForm.id" />

        <div class="row">
            <div class="col-auto">
                <input type="checkbox" id="edit-isCoplete" class="form-check-input" v-model="editForm.isComplete" />
            </div>
            <div class="col-auto">
                <input type="text" id="edit-name" class="form-control" v-model="editForm.name" />
            </div>
            <div class="col-auto">
                <button class="btn btn-primary" @click="updateItem()">Update</button>
            </div>
            <div class="col-auto">
                <button class="btn btn-secondary" @click="closeInput()">Close</button>
            </div>
        </div>
    </div>

    <div v-if="loading">
        <p>Loading...</p>
    </div>

    <div v-if="todoItems">
        <table class="table">
            <thead>
                <tr>
                    <th>id</th>
                    <th>name</th>
                    <th>is complete</th>
                    <th></th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="todo in todoItems" :key="todo.id">
                    <td>{{ todo.id }}</td>
                    <td>{{ todo.name }}</td>
                    <td>{{ todo.isComplete }}</td>
                    <td><button class="btn btn-primary" @click="displayEditForm(todo.id)">Edit</button></td>
                    <td><button class="btn btn-danger" @click="deleteItem(todo.id)">Delete</button></td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';

    type TodoList = {
        id: number,
        name: string,
        isComplete: boolean
    }[];

    type AddForm = {
        name: string,
        isComplete: boolean
    }

    type EditForm = {
        id: number,
        name: string,
        isComplete: boolean
    }

    interface Data {
        isShowAddForm: boolean,
        isShowEditForm: boolean,
        loading: boolean,
        todoItems: null | TodoList,
        addForm: AddForm,
        editForm: EditForm
    }

    export default defineComponent({
        data(): Data {
            return {
                isShowAddForm: true,
                isShowEditForm: false,
                loading: false,
                todoItems: null,
                addForm: {
                    name: '',
                    isComplete: false
                },
                editForm: {
                    id: -1,
                    name: '',
                    isComplete: false
                } 
            };
        },
        created() {
            console.log("created");
            this.fetchData();
        },
        watch: {
            // call again the method if the route changes
            '$route': 'fetchData'
        },
        methods: {
            fetchData(): void {
                console.log("fetchData");
                this.loading = true;
                this.todoItems = null;

                fetch('https://localhost:7174/api/TodoItems')
                .then(r => r.json())
                .then(json => {
                    console.log(json);
                    this.todoItems = json as TodoList;
                    this.loading = false;
                })
                .catch(error => console.error('Unable to get items.', error));
            },
            addItem(): void {
                console.log("addItem");
                const json = JSON.stringify(this.addForm);
                console.log(json);

                fetch('https://localhost:7174/api/TodoItems', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: json
                })
                .then(r => r.json())
                .then(() => {
                    this.fetchData();
                    this.addForm.name = '';
                })
                .catch(error => console.error('Unable to add item.', error));
            },
            updateItem(): void {
                console.log("updateItem");
                const json = JSON.stringify(this.editForm);
                console.log(json);

                fetch('https://localhost:7174/api/TodoItems/' + this.editForm.id, {
                    method: 'PUT',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: json
                })
                .then(() => {
                    this.fetchData();
                })
                .catch(error => console.error('Unable to update item.', error));

                this.closeInput();
            },
            closeInput(): void {
                console.log("closeInput");

                this.isShowAddForm = true;
                this.isShowEditForm = false;
            },
            displayEditForm(id: number): void {
                console.log("displayEditForm");

                const item = this.todoItems!.find(item => item.id === id);

                this.editForm.id = item!.id;
                this.editForm.name = item!.name;
                this.editForm.isComplete = item!.isComplete;

                this.isShowAddForm = false;
                this.isShowEditForm = true;
            },
            deleteItem(id: number): void {
                console.log("deleteItem");

                fetch(`https://localhost:7174/api/TodoItems/${id}`, {
                    method: 'DELETE',
                })
                .then(() => {
                    this.fetchData();
                })
                .catch(error => console.error('Unable to delete item.', error));            }
        },
    });
</script>

<style scoped lang="scss">
    $pad: .5rem;

    th {
        font-weight: bold;
    }

/*    tr:nth-child(even) {
        background: #F2F2F2;
    }

    tr:nth-child(odd) {
        background: #FFF;
    }*/

    th, td {
        padding-left: $pad;
        padding-right: $pad;
    }

    table {
        margin-left: auto;
        margin-right: auto;
    }

    #edit-isCoplete {
        vertical-align: middle;
    }
</style>