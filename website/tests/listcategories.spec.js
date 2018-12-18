/* eslint-disable no-undef */
/**
 * Shallow Mount is used to not mount the component's child components
 * for efficiency purposes and to not assert unwanted behaviour on any child components
 */
import { shallowMount } from '@vue/test-utils'

import ListCategories from '../src/components/management/category/ListCategories.vue'


describe('ListCategories Component', () =>{
    test('is a Vue instance', () =>{
        const wrapper = shallowMount(ListCategories);
        expect(wrapper.isVueInstance()).toBeTruthy();
    });
    //!This isn't actually testing the GET Request that is supposed to
    //TODO Test what is actually requested
    test('fetch all categories after clicking a button', (done)=>{
        const wrapper = shallowMount(ListCategories);
        wrapper.find('button').trigger('click');
        wrapper.vm.$nextTick(()=>{
            expect(wrapper.vm.availableCategories).toBe(Array);
            done();
        })
    })
})